using EventsAPI.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System.Threading.Tasks;

namespace EventsAPI.Validators
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required");
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Wrong email format")
                .Must(IsUnique).WithMessage("Current email is taken")
                .NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required")
                .Must(IsValidDate).WithMessage("Date must be earlier");
        }

        public bool IsValidDate(DateOnly date)
        {
            return date < DateOnly.FromDateTime(DateTime.Now);
        }

        public bool IsUnique(string email)
        {
            using (var db = new EventsDbContext())
            {
                if(db.Guests.Where(g => g.Email == email).SingleOrDefault() == null)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
