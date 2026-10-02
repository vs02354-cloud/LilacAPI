using System;
using System.IO;
using System.Threading.Tasks;
using LilacTechSys.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LilacTechSys.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedExtensions = { ".pdf" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveResumeFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file was uploaded.");

            if (file.Length > MaxFileSize)
                throw new InvalidOperationException("Resume file size must not exceed 5MB.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (Array.IndexOf(_allowedExtensions, ext) < 0)
                throw new InvalidOperationException("Only PDF documents (.pdf) are permitted for resume uploads.");

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "uploads", "resumes");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var safeFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var fullPath = Path.Combine(uploadsFolder, safeFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("uploads", "resumes", safeFileName).Replace('\\', '/');
        }

        public async Task<(byte[] Bytes, string ContentType, string FileName)?> GetResumeFileAsync(string relativePath)
        {
            var fullPath = Path.Combine(_env.ContentRootPath, relativePath.TrimStart('/', '\\'));
            if (!File.Exists(fullPath))
                return null;

            var bytes = await File.ReadAllBytesAsync(fullPath);
            return (bytes, "application/pdf", Path.GetFileName(fullPath));
        }

        public Task DeleteFileAsync(string relativePath)
        {
            var fullPath = Path.Combine(_env.ContentRootPath, relativePath.TrimStart('/', '\\'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            return Task.CompletedTask;
        }
    }
}
