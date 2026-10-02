using System.Threading.Tasks;
using LilacTechSys.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LilacTechSys.Infrastructure.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly ILogger<EmailNotificationService> _logger;
        private readonly IConfiguration _config;

        public EmailNotificationService(ILogger<EmailNotificationService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public Task SendContactNotificationAsync(string name, string email, string subject, string message)
        {
            _logger.LogInformation("[DISPATCH EMAIL] New Contact Inquiry from {Name} ({Email}). Subject: {Subject}. Message: {Message}",
                name, email, subject, message);
            return Task.CompletedTask;
        }

        public Task SendQuoteNotificationAsync(string name, string email, string service, string budget)
        {
            _logger.LogInformation("[DISPATCH EMAIL] New Project Quote Request from {Name} ({Email}). Service: {Service}, Budget: {Budget}",
                name, email, service, budget);
            return Task.CompletedTask;
        }

        public Task SendApplicationNotificationAsync(string name, string email, string jobTitle)
        {
            _logger.LogInformation("[DISPATCH EMAIL] New Job Application received from {Name} ({Email}) for position: {JobTitle}",
                name, email, jobTitle);
            return Task.CompletedTask;
        }
    }
}
