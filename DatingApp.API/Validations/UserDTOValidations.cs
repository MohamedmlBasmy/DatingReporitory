using DatingApp.API.DTOs;
using FluentValidation;

namespace DatingApp.API.Validations
{
    public class UserDTOValidations : AbstractValidator<UserDTO>
    {
        public UserDTOValidations()
        {
            RuleFor(x => x.Username).NotNull();
            //RuleFor(x => x.Password).NotEmpty().NotNull();
            //RuleFor(m => m.CustomerType).NotEmpty();
            //RuleFor(m => m.CustomerName).NotEmpty().MinimumLength(3).MaximumLength(100);
            //RuleFor(m => m.EmailAddress).EmailAddress();
            //RuleFor(m => m.FirstName).NotEmpty().When(m => m.CustomerType.ToLower() == "person");
        }
    }
}