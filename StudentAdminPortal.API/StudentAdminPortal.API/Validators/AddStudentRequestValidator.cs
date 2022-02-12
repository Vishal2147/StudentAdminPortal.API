using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Reopsitories;

namespace StudentAdminPortal.API.Validators
{
    public class AddStudentRequestValidator: AbstractValidator<AddStudentRequest>
    {

        public AddStudentRequestValidator(IStudentRepository studentRepository)
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.DateofBirth).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Mobile).GreaterThan(99999).LessThan(100000000000);
            RuleFor(x => x.GenderId).NotEmpty().Must(id =>
            {
                var gender = studentRepository.GetGenderAsync().Result.ToList()
                    .FirstOrDefault(x => x.Id == id);

                if (gender != null)
                {
                    return true;
                }

                return false;
            }).WithMessage("Please select a valid Gender");

            RuleFor(x => x.PhysicalAddress).NotEmpty();
            RuleFor(x => x.PostalAddress).NotEmpty();


        }
    }
}
