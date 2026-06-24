using FluentValidation;
using InvoiceSystem.API.Dtos.Request;

namespace InvoiceSystem.API.Validation
{
    public class InvoiceQueryRequestDtoValidator : AbstractValidator<InvoiceQueryRequestDto>
    {
        public InvoiceQueryRequestDtoValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .WithMessage("Search cannot exceed 100 characters.");
        }
    }
}