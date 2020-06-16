using DatingApp.API.DTOs;
using FluentValidation;

namespace DatingApp.API.Validations
{
    public class UserDTOValidations : AbstractValidator<UserDTO>
    {
        public UserDTOValidations()
        {
            RuleFor(x => x.Username).NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}