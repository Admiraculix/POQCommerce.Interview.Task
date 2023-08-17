using FluentValidation;
using PoqCommerce.Api.Models.Contracts.Requests;

namespace PoqCommerce.Api.Validators
{
    public class FilterObjectValidator : AbstractValidator<FilterObjectRequest>
    {
        public FilterObjectValidator()
        {
            RuleFor(filter => filter.MinPrice).GreaterThanOrEqualTo(0).WithMessage("Can't be negative number");
            RuleFor(filter => filter.MaxPrice).GreaterThan(0).WithMessage("Can't be negative number");
            RuleFor(filter => filter.Size)
                .Must(size => string.IsNullOrEmpty(size) || IsValidSize(size))
                .WithMessage("Invalid size. Allowed values: small, medium, large");
        }

        private bool IsValidSize(string size)
        {
            // Define the list of allowed sizes
            string[] allowedSizes = { "small", "medium", "large" };

            // Check if the provided size is in the allowed sizes list
            return allowedSizes.Contains(size?.ToLower());
        }
    }
}
