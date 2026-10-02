using Microsoft.EntityFrameworkCore;
using LilacTechSys.Domain.Entities;
using LilacTechSys.Application.Interfaces;

namespace LilacTechSys.Infrastructure.Data
{
    public class LilacDbContext : DbContext, IApplicationDbContext
    {
        public LilacDbContext(DbContextOptions<LilacDbContext> options) : base(options)
        {
        }

        public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
        public DbSet<Testimonial> Testimonials => Set<Testimonial>();
        public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
        public DbSet<JobOpening> JobOpenings => Set<JobOpening>();
        public DbSet<JobApplication> JobApplications => Set<JobApplication>();
        public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
        public DbSet<QuoteRequest> QuoteRequests => Set<QuoteRequest>();
        public DbSet<Subscriber> Subscribers => Set<Subscriber>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AdminUser
            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Username).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.FullName).HasMaxLength(150);
            });

            // Service
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Slug).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Icon).HasMaxLength(100);
            });

            // Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Slug).HasMaxLength(150).IsRequired();
            });

            // Project
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.Property(e => e.Title).HasMaxLength(250).IsRequired();
                entity.Property(e => e.Slug).HasMaxLength(250).IsRequired();
                entity.Property(e => e.ClientName).HasMaxLength(200);

                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Projects)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // BlogPost
            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.Property(e => e.Title).HasMaxLength(300).IsRequired();
                entity.Property(e => e.Slug).HasMaxLength(300).IsRequired();
                entity.Property(e => e.AuthorName).HasMaxLength(150);

                entity.HasOne(b => b.Category)
                      .WithMany(c => c.BlogPosts)
                      .HasForeignKey(b => b.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Testimonial
            modelBuilder.Entity<Testimonial>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ClientName).HasMaxLength(150).IsRequired();
                entity.Property(e => e.CompanyName).HasMaxLength(150);
            });

            // TeamMember
            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Role).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Department).HasMaxLength(100);
            });

            // JobOpening
            modelBuilder.Entity<JobOpening>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Slug).HasMaxLength(200).IsRequired();
            });

            // JobApplication
            modelBuilder.Entity<JobApplication>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ApplicantName).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.ResumeFilePath).HasMaxLength(500);

                entity.HasOne(a => a.JobOpening)
                      .WithMany(j => j.Applications)
                      .HasForeignKey(a => a.JobOpeningId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ContactMessage
            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.Subject).HasMaxLength(250);
            });

            // QuoteRequest
            modelBuilder.Entity<QuoteRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.Company).HasMaxLength(150);
            });

            // Subscriber
            modelBuilder.Entity<Subscriber>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            });
        }
    }
}
