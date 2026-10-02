using System.Threading;
using System.Threading.Tasks;
using LilacTechSys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LilacTechSys.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<AdminUser> AdminUsers { get; }
        DbSet<Service> Services { get; }
        DbSet<Category> Categories { get; }
        DbSet<Project> Projects { get; }
        DbSet<BlogPost> BlogPosts { get; }
        DbSet<Testimonial> Testimonials { get; }
        DbSet<TeamMember> TeamMembers { get; }
        DbSet<JobOpening> JobOpenings { get; }
        DbSet<JobApplication> JobApplications { get; }
        DbSet<ContactMessage> ContactMessages { get; }
        DbSet<QuoteRequest> QuoteRequests { get; }
        DbSet<Subscriber> Subscribers { get; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
