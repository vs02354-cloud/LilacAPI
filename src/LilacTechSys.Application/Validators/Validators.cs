using FluentValidation;
using LilacTechSys.Application.DTOs;

namespace LilacTechSys.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }

    public class SubmitContactRequestValidator : AbstractValidator<SubmitContactRequest>
    {
        public SubmitContactRequestValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(150).WithMessage("Full name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(x => x.Subject).NotEmpty().MaximumLength(250).WithMessage("Subject is required.");
            RuleFor(x => x.Message).NotEmpty().MinimumLength(10).WithMessage("Message must contain at least 10 characters.");
        }
    }

    public class SubmitQuoteRequestValidator : AbstractValidator<SubmitQuoteRequest>
    {
        public SubmitQuoteRequestValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(150).WithMessage("Full name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(x => x.ServiceRequired).NotEmpty().WithMessage("Please specify which service you require.");
            RuleFor(x => x.BudgetRange).NotEmpty().WithMessage("Please select an estimated budget range.");
            RuleFor(x => x.Timeline).NotEmpty().WithMessage("Please select an expected timeline.");
            RuleFor(x => x.ProjectDescription).NotEmpty().MinimumLength(15).WithMessage("Please describe your project in at least 15 characters.");
        }
    }

    public class SubmitApplicationRequestValidator : AbstractValidator<SubmitApplicationRequest>
    {
        public SubmitApplicationRequestValidator()
        {
            RuleFor(x => x.ApplicantName).NotEmpty().MaximumLength(150).WithMessage("Full name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Contact phone number is required.");
        }
    }

    public class SubscribeRequestValidator : AbstractValidator<SubscribeRequest>
    {
        public SubscribeRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Please provide a valid email address.");
        }
    }

    public class CreateServiceRequestValidator : AbstractValidator<CreateServiceRequest>
    {
        public CreateServiceRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(500);
            RuleFor(x => x.DetailedDescription).NotEmpty();
        }
    }

    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
    {
        public CreateProjectRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Summary).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }

    public class CreateBlogRequestValidator : AbstractValidator<CreateBlogRequest>
    {
        public CreateBlogRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(300);
            RuleFor(x => x.Excerpt).NotEmpty();
            RuleFor(x => x.ContentHtml).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
