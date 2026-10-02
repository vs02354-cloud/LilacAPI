using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using FluentValidation.TestHelper;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Validators;
using LilacTechSys.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LilacTechSys.Tests
{
    public class ValidationAndSecurityTests
    {
        private readonly SubmitContactRequestValidator _contactValidator = new();
        private readonly SubmitQuoteRequestValidator _quoteValidator = new();
        private readonly LoginRequestValidator _loginValidator = new();

        [Fact]
        public void ContactValidator_ValidRequest_ShouldNotHaveErrors()
        {
            var req = new SubmitContactRequest
            {
                FullName = "Alexander Wright",
                Email = "alexander@vantagecloud.io",
                Subject = "Enterprise Kubernetes Cloud Migration Inquiry",
                Message = "We are exploring a multi-region zero-trust cloud migration for our banking systems."
            };

            var result = _contactValidator.TestValidate(req);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ContactValidator_InvalidEmail_ShouldHaveValidationError()
        {
            var req = new SubmitContactRequest
            {
                FullName = "Alexander Wright",
                Email = "invalid-email-format",
                Subject = "Subject",
                Message = "Valid message length exceeding ten characters."
            };

            var result = _contactValidator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void QuoteValidator_ValidRequest_ShouldPass()
        {
            var req = new SubmitQuoteRequest
            {
                FullName = "Elena Rostova",
                Email = "elena@globallogistics.com",
                ServiceRequired = "Cloud & DevOps",
                BudgetRange = "$25k - $50k",
                Timeline = "1-3 Months",
                ProjectDescription = "Architecture overhaul for our real-time telemetry mesh across international shipping routes."
            };

            var result = _quoteValidator.TestValidate(req);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void QuoteValidator_EmptyService_ShouldFail()
        {
            var req = new SubmitQuoteRequest
            {
                FullName = "Elena Rostova",
                Email = "elena@globallogistics.com",
                ServiceRequired = "",
                BudgetRange = "$25k - $50k",
                Timeline = "1-3 Months",
                ProjectDescription = "Architecture overhaul for our real-time telemetry mesh."
            };

            var result = _quoteValidator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.ServiceRequired);
        }

        [Fact]
        public void JwtTokenGenerator_GeneratesValidTokenWithExpectedClaims()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "JwtSettings:Secret", "LilacTechSysUltraSecureKeyForJwtTokenGeneration2026!#*99248572019" },
                { "JwtSettings:Issuer", "LilacTechSys.Api" },
                { "JwtSettings:Audience", "LilacTechSys.Frontend" },
                { "JwtSettings:ExpiryMinutes", "60" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var tokenGenerator = new JwtTokenGenerator(configuration);
            var userId = Guid.NewGuid();
            var username = "admin";
            var email = "admin@lilactechsys.com";
            var role = "SuperAdmin";

            var token = tokenGenerator.GenerateAccessToken(userId, username, email, role);
            Assert.False(string.IsNullOrWhiteSpace(token));

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            Assert.Equal("LilacTechSys.Api", jwt.Issuer);
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId.ToString());
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Role && c.Value == role);

            var refreshToken = tokenGenerator.GenerateRefreshToken();
            Assert.False(string.IsNullOrWhiteSpace(refreshToken));
            Assert.True(refreshToken.Length > 20);
        }
    }
}
