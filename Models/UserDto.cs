using FluentValidation;
using TodoAPI.Services;

namespace TodoAPI.Models;

public class UserDto
{
    public string Name { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Role { get; set; }

    public static List<UniqueAttribute<string>> GetUniqueAttributes(UserService service, UserDto userDto)
    {
        return
        [
            new UniqueAttribute<string>
            (
                name: "Email", serviceSearcherMethod:
                service.GetByEmail!,
                value: userDto.Email
            ),
            new UniqueAttribute<string>
            (
                name: "Username",
                serviceSearcherMethod: service.GetByUsername!,
                value: userDto.Username
            )
        ];
    }

    public User ToUser()
    {
        return new User
        {
            Name = Name,
            Username = Username,
            Email = Email,
            Password = Password,
            Role = Role
        };
    }

    public class Validator : AbstractValidator<UserDto>
    {
        public Validator()
        {
            RuleFor(x => x.Name).Length(2, 64).NotNull();
            RuleFor(x => x.Username).Length(2, 32).NotNull();
            RuleFor(x => x.Email).EmailAddress().MaximumLength(64).NotNull();
            RuleFor(x => x.Password).Length(8, 64).NotNull();
            RuleFor(x => x.Role)
                .Must(role => role is "admin" or "commom").WithMessage("Should be 'admin' or 'commom'").NotNull();
        }
    }
}