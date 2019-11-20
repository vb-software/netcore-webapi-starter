using System;
using FluentValidation;

namespace RootNamespace.Entities.DTO
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(o => o.FirstName).NotEmpty();
            RuleFor(o => o.LastName).NotEmpty();
            RuleFor(o => o.DateOfBirth).NotEmpty();
        }
    }
}