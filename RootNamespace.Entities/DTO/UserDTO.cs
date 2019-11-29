using System;
using FluentValidation;

namespace RootNamespace.Entities.DTO
{
    public class UserDto
    {
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(o => o.Guid).NotEqual(Guid.Empty);
            RuleFor(o => o.FirstName).NotEmpty();
            RuleFor(o => o.LastName).NotEmpty();
        }
    }
}