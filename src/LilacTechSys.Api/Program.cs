using System;
using System.Text;
using FluentValidation;
using LilacTechSys.Api.Middleware;
using LilacTechSys.Application.Interfaces;
using LilacTechSys.Application.Services;
using LilacTechSys.Application.Validators;
using LilacTechSys.Infrastructure.Data;
using LilacTechSys.Infrastructure.Repositories;
using LilacTechSys.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure port for Render or dynamic hosting
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Database configuration with Render/Cloud DATABASE_URL support
var rawConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=127.0.0.1;Port=5432;Database=lilactechsys_database;Username=postgres;Password=postgres;";

var connectionString = ParseDatabaseUrl(rawConnectionString);

// Safe diagnostic logging (mask password)
var maskedConnStr = System.Text.RegularExpressions.Regex.Replace(connectionString, @"Password=[^;]+", "Password=******");
Log.Information("Configured database connection: {ConnectionString}", maskedConnStr);

if (connectionString.Contains("Host=127.0.0.1") && !builder.Environment.IsDevelopment())
{
    Log.Warning("Running in non-development environment with localhost database! Set DATABASE_URL environment variable on Render.");
}

builder.Services.AddDbContext<LilacDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("LilacTechSys.Infrastructure")));
builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<LilacDbContext>());

// Repositories & Unit of Work
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Infrastructure & Application Services
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ICareerService, CareerService>();
builder.Services.AddScoped<IContactQuoteService, ContactQuoteService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

// JWT Authentication
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
    ?? builder.Configuration["JwtSettings:Secret"]
    ?? "LilacTechSysUltraSecureKeyForJwtTokenGeneration2026!#*99248572019";
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "LilacTechSys.Api";
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "LilacTechSys.Frontend";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT Support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LilacTechSys API",
        Version = "v1",
        Description = "Production RESTful API backend for LilacTechSys IT Solutions, Digital Services & Admin Console",
        Contact = new OpenApiContact
        {
            Name = "LilacTechSys Engineering",
            Email = "contact@lilactechsys.com",
            Url = new Uri("https://lilactechsys.com")
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Include XML comments if present
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (System.IO.File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Auto-migrate and Seed database on launch
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LilacDbContext>();
    try
    {
        db.Database.EnsureCreated();
        await SeedData.SeedAsync(db);
        Log.Information("Database successfully initialized and seeded with sample data.");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while seeding the database: {Message}", ex.Message);
    }
}

// Middleware Pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();

// Always enable Swagger in both Development and Production for developer & admin access
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LilacTechSys API v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "LilacTechSys API - Documentation";
    c.EnablePersistAuthorization();
    c.DisplayRequestDuration();
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
});

// Root path redirects directly to /swagger for convenient browser access
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static string ParseDatabaseUrl(string connStr)
{
    if (string.IsNullOrWhiteSpace(connStr)) return string.Empty;

    if (!connStr.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) &&
        !connStr.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        // Append SSL and error detail if not already specified
        var res = connStr;
        if (!res.Contains("Trust Server Certificate", StringComparison.OrdinalIgnoreCase))
            res += ";Trust Server Certificate=true";
        if (!res.Contains("Include Error Detail", StringComparison.OrdinalIgnoreCase))
            res += ";Include Error Detail=true";
        return res;
    }

    try
    {
        // Strip scheme (postgres:// or postgresql://)
        var schemeIdx = connStr.IndexOf("://", StringComparison.Ordinal);
        var rest = connStr.Substring(schemeIdx + 3);

        // Strip query parameters (?sslmode=require, etc.)
        var queryIdx = rest.IndexOf('?');
        var main = queryIdx >= 0 ? rest.Substring(0, queryIdx) : rest;

        // The last '@' strictly separates credentials from host:port/database
        var atIdx = main.LastIndexOf('@');
        if (atIdx < 0) return connStr;

        var userInfo = main.Substring(0, atIdx);
        var hostDb = main.Substring(atIdx + 1);

        // First ':' in userInfo separates username from password
        var colonIdx = userInfo.IndexOf(':');
        var user = colonIdx >= 0 ? Uri.UnescapeDataString(userInfo.Substring(0, colonIdx)) : Uri.UnescapeDataString(userInfo);
        var password = colonIdx >= 0 ? Uri.UnescapeDataString(userInfo.Substring(colonIdx + 1)) : "";

        // First '/' in hostDb separates host[:port] from database
        var slashIdx = hostDb.IndexOf('/');
        var hostPort = slashIdx >= 0 ? hostDb.Substring(0, slashIdx) : hostDb;
        var database = slashIdx >= 0 ? hostDb.Substring(slashIdx + 1) : "";

        var port = 5432;
        var host = hostPort;
        var hpColonIdx = hostPort.LastIndexOf(':');
        if (hpColonIdx >= 0 && int.TryParse(hostPort.Substring(hpColonIdx + 1), out var parsedPort))
        {
            port = parsedPort;
            host = hostPort.Substring(0, hpColonIdx);
        }

        var isLocal = host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase) || host.Equals("localhost", StringComparison.OrdinalIgnoreCase);
        var sslMode = isLocal ? "Disable" : "Require";

        return $"Host={host};Port={port};Database={database};Username={user};Password={password};SSL Mode={sslMode};Trust Server Certificate=true;Include Error Detail=true;";
    }
    catch
    {
        return connStr;
    }
}

