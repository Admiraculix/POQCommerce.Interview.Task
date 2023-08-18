using FluentValidation;
using PoqCommerce.Api.Models.Contracts.Requests;

namespace PoqCommerce.Api.Validators
{
    public class FilterObjectValidator : AbstractValidator<FilterObjectRequest>
    {
        private const string CantBeNegativeNumber = "Can't be negative number";
        private const string InvalidSize = "Invalid size. Allowed values: small, medium, large";

        public FilterObjectValidator()
        {
            RuleFor(filter => filter.MinPrice).GreaterThanOrEqualTo(0).WithMessage(CantBeNegativeNumber);
            RuleFor(filter => filter.MaxPrice).GreaterThan(0).WithMessage(CantBeNegativeNumber);
            RuleFor(filter => filter.Size)
                .Must(size => string.IsNullOrEmpty(size) || IsValidSize(size))
                .WithMessage(InvalidSize);
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
