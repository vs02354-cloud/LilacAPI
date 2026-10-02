using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LilacTechSys.Application.Common;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Interfaces;
using LilacTechSys.Domain.Entities;

namespace LilacTechSys.Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IRepository<Service> _serviceRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceService(IRepository<Service> serviceRepo, IUnitOfWork unitOfWork)
        {
            _serviceRepo = serviceRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<List<ServiceDto>>> GetAllServicesAsync(bool includeInactive = false)
        {
            var services = includeInactive
                ? await _serviceRepo.GetAllAsync()
                : await _serviceRepo.FindAsync(s => s.IsActive);

            var dtos = services.OrderBy(s => s.DisplayOrder)
                .Select(MapToDto)
                .ToList();

            return ApiResponse<List<ServiceDto>>.Ok(dtos);
        }

        public async Task<ApiResponse<ServiceDto>> GetServiceBySlugOrIdAsync(string slugOrId)
        {
            Service? service = null;
            if (Guid.TryParse(slugOrId, out var id))
            {
                service = await _serviceRepo.GetByIdAsync(id);
            }
            else
            {
                var matches = await _serviceRepo.FindAsync(s => s.Slug.ToLower() == slugOrId.ToLower());
                service = matches.FirstOrDefault();
            }

            if (service == null)
                return ApiResponse<ServiceDto>.Fail("Service not found.");

            return ApiResponse<ServiceDto>.Ok(MapToDto(service));
        }

        public async Task<ApiResponse<ServiceDto>> CreateServiceAsync(CreateServiceRequest request)
        {
            var existing = await _serviceRepo.FindAsync(s => s.Slug.ToLower() == request.Slug.ToLower());
            if (existing.Any())
                return ApiResponse<ServiceDto>.Fail("A service with this URL slug already exists.");

            var service = new Service
            {
                Title = request.Title,
                Slug = request.Slug.ToLower().Trim(),
                ShortDescription = request.ShortDescription,
                DetailedDescription = request.DetailedDescription,
                Icon = request.Icon,
                FeaturesJson = JsonSerializer.Serialize(request.Features ?? new List<string>()),
                BenefitsJson = JsonSerializer.Serialize(request.Benefits ?? new List<string>()),
                TechnologiesJson = JsonSerializer.Serialize(request.Technologies ?? new List<string>()),
                DisplayOrder = request.DisplayOrder,
                IsActive = request.IsActive
            };

            await _serviceRepo.AddAsync(service);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<ServiceDto>.Ok(MapToDto(service), "Service created successfully.");
        }

        public async Task<ApiResponse<ServiceDto>> UpdateServiceAsync(Guid id, UpdateServiceRequest request)
        {
            var service = await _serviceRepo.GetByIdAsync(id);
            if (service == null)
                return ApiResponse<ServiceDto>.Fail("Service not found.");

            service.Title = request.Title;
            service.Slug = request.Slug.ToLower().Trim();
            service.ShortDescription = request.ShortDescription;
            service.DetailedDescription = request.DetailedDescription;
            service.Icon = request.Icon;
            service.FeaturesJson = JsonSerializer.Serialize(request.Features ?? new List<string>());
            service.BenefitsJson = JsonSerializer.Serialize(request.Benefits ?? new List<string>());
            service.TechnologiesJson = JsonSerializer.Serialize(request.Technologies ?? new List<string>());
            service.DisplayOrder = request.DisplayOrder;
            service.IsActive = request.IsActive;

            await _serviceRepo.UpdateAsync(service);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<ServiceDto>.Ok(MapToDto(service), "Service updated successfully.");
        }

        public async Task<ApiResponse> DeleteServiceAsync(Guid id)
        {
            var service = await _serviceRepo.GetByIdAsync(id);
            if (service == null)
                return ApiResponse.ErrorResult("Service not found.");

            await _serviceRepo.DeleteAsync(service);
            await _unitOfWork.CompleteAsync();

            return ApiResponse.SuccessResult("Service deleted successfully.");
        }

        private static ServiceDto MapToDto(Service s)
        {
            return new ServiceDto
            {
                Id = s.Id,
                Title = s.Title,
                Slug = s.Slug,
                ShortDescription = s.ShortDescription,
                DetailedDescription = s.DetailedDescription,
                Icon = s.Icon,
                Features = SafeDeserializeList(s.FeaturesJson),
                Benefits = SafeDeserializeList(s.BenefitsJson),
                Technologies = SafeDeserializeList(s.TechnologiesJson),
                DisplayOrder = s.DisplayOrder,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt
            };
        }

        private static List<string> SafeDeserializeList(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<string>();
            try { return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>(); }
            catch { return new List<string>(); }
        }
    }
}
