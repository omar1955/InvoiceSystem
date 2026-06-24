using FluentValidation;
using InvoiceSystem.API.Dtos.Request;

namespace InvoiceSystem.API.Validation
{
    public class UpdateInvoiceRequestDtoValidator : AbstractValidator<UpdateInvoiceRequestDto>
    {
        public UpdateInvoiceRequestDtoValidator()
        {
            RuleFor(x => x.StoreId)
                .GreaterThan(0)
                .WithMessage("Store is required.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Customer is required.");

            RuleFor(x => x.Note)
                .MaximumLength(500)
                .WithMessage("Note cannot exceed 500 characters.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Invoice must contain at least one item.");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateInvoiceItemRequestDtoValidator());
        }
    }
}