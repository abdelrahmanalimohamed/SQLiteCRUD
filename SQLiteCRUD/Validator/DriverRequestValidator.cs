using FluentValidation;
using SQLiteCRUD.Models;

namespace SQLiteCRUD.Validator
{
    public class DriverRequestValidator : AbstractValidator<DriverRequest>
    {
        public DriverRequestValidator()
        {
            RuleFor(driver => driver.firstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(driver => driver.lastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(driver => driver.email).NotEmpty().EmailAddress().WithMessage("Invalid email address.");
            RuleFor(driver => driver.phoneNumber).NotEmpty().WithMessage("Phone number is required.");
        }
    }
}
