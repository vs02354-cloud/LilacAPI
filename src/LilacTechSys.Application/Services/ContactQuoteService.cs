using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LilacTechSys.Application.Common;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Interfaces;
using LilacTechSys.Domain.Entities;
using LilacTechSys.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LilacTechSys.Application.Services
{
    public class ContactQuoteService : IContactQuoteService
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailNotificationService _emailService;

        public ContactQuoteService(IApplicationDbContext context, IEmailNotificationService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<ApiResponse> SubmitContactAsync(SubmitContactRequest request)
        {
            var msg = new ContactMessage
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                Subject = request.Subject,
                Message = request.Message,
                IsRead = false
            };

            _context.ContactMessages.Add(msg);
            await _context.SaveChangesAsync();

            await _emailService.SendContactNotificationAsync(request.FullName, request.Email, request.Subject, request.Message);

            return ApiResponse.SuccessResult("Thank you for contacting LilacTechSys. Our solutions team will respond within 24 hours.");
        }

        public async Task<ApiResponse<PagedResult<ContactMessageDto>>> GetContactMessagesAsync(QueryParameters queryParams)
        {
            var query = _context.ContactMessages.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var term = queryParams.SearchTerm.ToLower();
                query = query.Where(m => m.FullName.ToLower().Contains(term) ||
                                         m.Email.ToLower().Contains(term) ||
                                         m.Subject.ToLower().Contains(term));
            }

            var totalCount = await query.CountAsync();

            var items = await query.OrderByDescending(m => m.CreatedAt)
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(m => new ContactMessageDto
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    Email = m.Email,
                    Phone = m.Phone,
                    Subject = m.Subject,
                    Message = m.Message,
                    IsRead = m.IsRead,
                    RespondedAt = m.RespondedAt,
                    ResponseNotes = m.ResponseNotes,
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync();

            return ApiResponse<PagedResult<ContactMessageDto>>.Ok(
                new PagedResult<ContactMessageDto>(items, totalCount, queryParams.PageNumber, queryParams.PageSize));
        }

        public async Task<ApiResponse> MarkContactAsReadAsync(Guid id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg == null) return ApiResponse.ErrorResult("Message not found.");

            msg.IsRead = true;
            await _context.SaveChangesAsync();
            return ApiResponse.SuccessResult("Marked as read.");
        }

        public async Task<ApiResponse<QuoteRequestDto>> SubmitQuoteAsync(SubmitQuoteRequest request)
        {
            var quote = new QuoteRequest
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                Company = request.Company,
                ServiceRequired = request.ServiceRequired,
                BudgetRange = request.BudgetRange,
                Timeline = request.Timeline,
                ProjectDescription = request.ProjectDescription,
                Status = QuoteStatus.New
            };

            _context.QuoteRequests.Add(quote);
            await _context.SaveChangesAsync();

            await _emailService.SendQuoteNotificationAsync(request.FullName, request.Email, request.ServiceRequired, request.BudgetRange);

            var dto = new QuoteRequestDto
            {
                Id = quote.Id,
                FullName = quote.FullName,
                Email = quote.Email,
                Phone = quote.Phone,
                Company = quote.Company,
                ServiceRequired = quote.ServiceRequired,
                BudgetRange = quote.BudgetRange,
                Timeline = quote.Timeline,
                ProjectDescription = quote.ProjectDescription,
                Status = quote.Status,
                CreatedAt = quote.CreatedAt
            };

            return ApiResponse<QuoteRequestDto>.Ok(dto, "Quote request submitted successfully. A specialist will prepare your custom proposal.");
        }

        public async Task<ApiResponse<PagedResult<QuoteRequestDto>>> GetQuoteRequestsAsync(QueryParameters queryParams)
        {
            var query = _context.QuoteRequests.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var term = queryParams.SearchTerm.ToLower();
                query = query.Where(q => q.FullName.ToLower().Contains(term) ||
                                         q.Email.ToLower().Contains(term) ||
                                         (q.Company != null && q.Company.ToLower().Contains(term)));
            }

            var totalCount = await query.CountAsync();

            var items = await query.OrderByDescending(q => q.CreatedAt)
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(q => new QuoteRequestDto
                {
                    Id = q.Id,
                    FullName = q.FullName,
                    Email = q.Email,
                    Phone = q.Phone,
                    Company = q.Company,
                    ServiceRequired = q.ServiceRequired,
                    BudgetRange = q.BudgetRange,
                    Timeline = q.Timeline,
                    ProjectDescription = q.ProjectDescription,
                    Status = q.Status,
                    AdminNotes = q.AdminNotes,
                    EstimatedQuoteAmount = q.EstimatedQuoteAmount,
                    CreatedAt = q.CreatedAt
                })
                .ToListAsync();

            return ApiResponse<PagedResult<QuoteRequestDto>>.Ok(
                new PagedResult<QuoteRequestDto>(items, totalCount, queryParams.PageNumber, queryParams.PageSize));
        }

        public async Task<ApiResponse> UpdateQuoteStatusAsync(Guid id, UpdateQuoteStatusRequest request)
        {
            var quote = await _context.QuoteRequests.FindAsync(id);
            if (quote == null) return ApiResponse.ErrorResult("Quote request not found.");

            quote.Status = request.Status;
            quote.AdminNotes = request.AdminNotes;
            quote.EstimatedQuoteAmount = request.EstimatedQuoteAmount;
            quote.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ApiResponse.SuccessResult("Quote request updated.");
        }

        public async Task<ApiResponse> SubscribeNewsletterAsync(SubscribeRequest request)
        {
            var exists = await _context.Subscribers.AnyAsync(s => s.Email.ToLower() == request.Email.ToLower());
            if (exists)
            {
                return ApiResponse.SuccessResult("You are already subscribed to LilacTechSys insights.");
            }

            var sub = new Subscriber
            {
                Email = request.Email.ToLower().Trim(),
                IsActive = true,
                SubscribedAt = DateTime.UtcNow
            };

            _context.Subscribers.Add(sub);
            await _context.SaveChangesAsync();

            return ApiResponse.SuccessResult("Thank you for subscribing to LilacTechSys Tech Radar.");
        }

        public async Task<ApiResponse<List<SubscriberDto>>> GetSubscribersAsync()
        {
            var subs = await _context.Subscribers
                .AsNoTracking()
                .OrderByDescending(s => s.SubscribedAt)
                .Select(s => new SubscriberDto
                {
                    Id = s.Id,
                    Email = s.Email,
                    IsActive = s.IsActive,
                    SubscribedAt = s.SubscribedAt
                })
                .ToListAsync();

            return ApiResponse<List<SubscriberDto>>.Ok(subs);
        }

        public async Task<ApiResponse<List<TestimonialDto>>> GetTestimonialsAsync(bool featuredOnly = false)
        {
            var query = _context.Testimonials.AsNoTracking().AsQueryable();
            if (featuredOnly) query = query.Where(t => t.IsFeatured);

            var items = await query.OrderBy(t => t.DisplayOrder)
                .Select(t => new TestimonialDto
                {
                    Id = t.Id,
                    ClientName = t.ClientName,
                    ClientTitle = t.ClientTitle,
                    CompanyName = t.CompanyName,
                    AvatarUrl = t.AvatarUrl,
                    Rating = t.Rating,
                    Content = t.Content,
                    ProjectName = t.ProjectName,
                    IsFeatured = t.IsFeatured,
                    DisplayOrder = t.DisplayOrder
                })
                .ToListAsync();

            return ApiResponse<List<TestimonialDto>>.Ok(items);
        }

        public async Task<ApiResponse<TestimonialDto>> CreateTestimonialAsync(CreateTestimonialRequest request)
        {
            var t = new Testimonial
            {
                ClientName = request.ClientName,
                ClientTitle = request.ClientTitle,
                CompanyName = request.CompanyName,
                AvatarUrl = request.AvatarUrl,
                Rating = request.Rating,
                Content = request.Content,
                ProjectName = request.ProjectName,
                IsFeatured = request.IsFeatured,
                DisplayOrder = request.DisplayOrder
            };

            _context.Testimonials.Add(t);
            await _context.SaveChangesAsync();

            return ApiResponse<TestimonialDto>.Ok(new TestimonialDto
            {
                Id = t.Id,
                ClientName = t.ClientName,
                ClientTitle = t.ClientTitle,
                CompanyName = t.CompanyName,
                AvatarUrl = t.AvatarUrl,
                Rating = t.Rating,
                Content = t.Content,
                ProjectName = t.ProjectName,
                IsFeatured = t.IsFeatured,
                DisplayOrder = t.DisplayOrder
            }, "Testimonial added.");
        }

        public async Task<ApiResponse> DeleteTestimonialAsync(Guid id)
        {
            var t = await _context.Testimonials.FindAsync(id);
            if (t == null) return ApiResponse.ErrorResult("Testimonial not found.");

            _context.Testimonials.Remove(t);
            await _context.SaveChangesAsync();
            return ApiResponse.SuccessResult("Testimonial deleted.");
        }

        public async Task<ApiResponse<List<TeamMemberDto>>> GetTeamMembersAsync()
        {
            var items = await _context.TeamMembers
                .AsNoTracking()
                .Where(m => m.IsActive)
                .OrderBy(m => m.DisplayOrder)
                .Select(m => new TeamMemberDto
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    Role = m.Role,
                    Bio = m.Bio,
                    AvatarUrl = m.AvatarUrl,
                    Department = m.Department,
                    LinkedInUrl = m.LinkedInUrl,
                    TwitterUrl = m.TwitterUrl,
                    GithubUrl = m.GithubUrl,
                    DisplayOrder = m.DisplayOrder,
                    IsActive = m.IsActive
                })
                .ToListAsync();

            return ApiResponse<List<TeamMemberDto>>.Ok(items);
        }

        public async Task<ApiResponse<TeamMemberDto>> CreateTeamMemberAsync(CreateTeamMemberRequest request)
        {
            var m = new TeamMember
            {
                FullName = request.FullName,
                Role = request.Role,
                Bio = request.Bio,
                AvatarUrl = request.AvatarUrl,
                Department = request.Department,
                LinkedInUrl = request.LinkedInUrl,
                TwitterUrl = request.TwitterUrl,
                GithubUrl = request.GithubUrl,
                DisplayOrder = request.DisplayOrder,
                IsActive = request.IsActive
            };

            _context.TeamMembers.Add(m);
            await _context.SaveChangesAsync();

            return ApiResponse<TeamMemberDto>.Ok(new TeamMemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Role = m.Role,
                Bio = m.Bio,
                AvatarUrl = m.AvatarUrl,
                Department = m.Department,
                LinkedInUrl = m.LinkedInUrl,
                TwitterUrl = m.TwitterUrl,
                GithubUrl = m.GithubUrl,
                DisplayOrder = m.DisplayOrder,
                IsActive = m.IsActive
            }, "Team member added.");
        }

        public async Task<ApiResponse> DeleteTeamMemberAsync(Guid id)
        {
            var m = await _context.TeamMembers.FindAsync(id);
            if (m == null) return ApiResponse.ErrorResult("Team member not found.");

            _context.TeamMembers.Remove(m);
            await _context.SaveChangesAsync();
            return ApiResponse.SuccessResult("Team member removed.");
        }
    }
}
