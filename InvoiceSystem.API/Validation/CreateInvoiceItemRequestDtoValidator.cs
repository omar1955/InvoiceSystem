using FluentValidation;
using InvoiceSystem.API.Dtos.Request;

namespace InvoiceSystem.API.Validation
{
    public class CreateInvoiceItemRequestDtoValidator : AbstractValidator<CreateInvoiceItemRequestDto>
    {
        public CreateInvoiceItemRequestDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("Discount percentage must be between 0 and 100.");

            RuleFor(x => x.TaxPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("Tax percentage must be between 0 and 100.");
        }
    }
}