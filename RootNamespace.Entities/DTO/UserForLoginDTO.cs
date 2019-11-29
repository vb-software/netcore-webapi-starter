using FluentValidation;

namespace RootNamespace.Entities.DTO
{
    public class UserForLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserForLoginDtoValidator : AbstractValidator<UserForLoginDto>
    {
        public UserForLoginDtoValidator()
        {
            RuleFor(o => o.Username).NotEmpty();
            RuleFor(o => o.Password).NotEmpty();
        }
    }
}