using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LilacTechSys.Application.Common;
using LilacTechSys.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace LilacTechSys.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<ApiResponse<AdminUserDto>> GetCurrentUserAsync(Guid userId);
    }

    public interface IServiceService
    {
        Task<ApiResponse<List<ServiceDto>>> GetAllServicesAsync(bool includeInactive = false);
        Task<ApiResponse<ServiceDto>> GetServiceBySlugOrIdAsync(string slugOrId);
        Task<ApiResponse<ServiceDto>> CreateServiceAsync(CreateServiceRequest request);
        Task<ApiResponse<ServiceDto>> UpdateServiceAsync(Guid id, UpdateServiceRequest request);
        Task<ApiResponse> DeleteServiceAsync(Guid id);
    }

    public interface IProjectService
    {
        Task<ApiResponse<PagedResult<ProjectDto>>> GetProjectsAsync(QueryParameters queryParams);
        Task<ApiResponse<ProjectDetailDto>> GetProjectBySlugOrIdAsync(string slugOrId);
        Task<ApiResponse<List<ProjectDto>>> GetFeaturedProjectsAsync();
        Task<ApiResponse<ProjectDto>> CreateProjectAsync(CreateProjectRequest request);
        Task<ApiResponse<ProjectDto>> UpdateProjectAsync(Guid id, UpdateProjectRequest request);
        Task<ApiResponse> DeleteProjectAsync(Guid id);
    }

    public interface IBlogService
    {
        Task<ApiResponse<PagedResult<BlogPostDto>>> GetBlogPostsAsync(QueryParameters queryParams);
        Task<ApiResponse<BlogPostDetailDto>> GetBlogPostBySlugOrIdAsync(string slugOrId);
        Task<ApiResponse<List<BlogPostDto>>> GetRecentBlogPostsAsync(int count = 3);
        Task<ApiResponse<List<CategoryDto>>> GetCategoriesAsync();
        Task<ApiResponse<CategoryDto>> CreateCategoryAsync(CreateCategoryRequest request);
        Task<ApiResponse<BlogPostDto>> CreateBlogPostAsync(CreateBlogRequest request);
        Task<ApiResponse<BlogPostDto>> UpdateBlogPostAsync(Guid id, UpdateBlogRequest request);
        Task<ApiResponse> DeleteBlogPostAsync(Guid id);
    }

    public interface ICareerService
    {
        Task<ApiResponse<List<JobOpeningDto>>> GetActiveJobOpeningsAsync();
        Task<ApiResponse<JobOpeningDto>> GetJobOpeningBySlugOrIdAsync(string slugOrId);
        Task<ApiResponse<JobOpeningDto>> CreateJobOpeningAsync(CreateJobOpeningRequest request);
        Task<ApiResponse<JobOpeningDto>> UpdateJobOpeningAsync(Guid id, CreateJobOpeningRequest request);
        Task<ApiResponse> DeleteJobOpeningAsync(Guid id);

        Task<ApiResponse<JobApplicationDto>> SubmitApplicationAsync(Guid jobOpeningId, SubmitApplicationRequest request);
        Task<ApiResponse<PagedResult<JobApplicationDto>>> GetApplicationsAsync(QueryParameters queryParams);
        Task<ApiResponse> UpdateApplicationStatusAsync(Guid applicationId, UpdateApplicationStatusRequest request);
    }

    public interface IContactQuoteService
    {
        Task<ApiResponse> SubmitContactAsync(SubmitContactRequest request);
        Task<ApiResponse<PagedResult<ContactMessageDto>>> GetContactMessagesAsync(QueryParameters queryParams);
        Task<ApiResponse> MarkContactAsReadAsync(Guid id);

        Task<ApiResponse<QuoteRequestDto>> SubmitQuoteAsync(SubmitQuoteRequest request);
        Task<ApiResponse<PagedResult<QuoteRequestDto>>> GetQuoteRequestsAsync(QueryParameters queryParams);
        Task<ApiResponse> UpdateQuoteStatusAsync(Guid id, UpdateQuoteStatusRequest request);

        Task<ApiResponse> SubscribeNewsletterAsync(SubscribeRequest request);
        Task<ApiResponse<List<SubscriberDto>>> GetSubscribersAsync();

        Task<ApiResponse<List<TestimonialDto>>> GetTestimonialsAsync(bool featuredOnly = false);
        Task<ApiResponse<TestimonialDto>> CreateTestimonialAsync(CreateTestimonialRequest request);
        Task<ApiResponse> DeleteTestimonialAsync(Guid id);

        Task<ApiResponse<List<TeamMemberDto>>> GetTeamMembersAsync();
        Task<ApiResponse<TeamMemberDto>> CreateTeamMemberAsync(CreateTeamMemberRequest request);
        Task<ApiResponse> DeleteTeamMemberAsync(Guid id);
    }

    public interface IDashboardService
    {
        Task<ApiResponse<DashboardStatsDto>> GetDashboardStatsAsync();
    }

    public interface IFileStorageService
    {
        Task<string> SaveResumeFileAsync(IFormFile file);
        Task<(byte[] Bytes, string ContentType, string FileName)?> GetResumeFileAsync(string relativePath);
        Task DeleteFileAsync(string relativePath);
    }

    public interface IEmailNotificationService
    {
        Task SendContactNotificationAsync(string name, string email, string subject, string message);
        Task SendQuoteNotificationAsync(string name, string email, string service, string budget);
        Task SendApplicationNotificationAsync(string name, string email, string jobTitle);
    }

    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(Guid userId, string username, string email, string role);
        string GenerateRefreshToken();
    }
}
