using FluentValidation;
using Sample.EmployeeService.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Core.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator() 
        {
            RuleFor(x => x.Name).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Name can not be empty")
                .NotNull().WithMessage("Name can not be null");

            RuleFor(x => x.Age).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Age can not be empty")
                .GreaterThan(0).WithMessage("Age need to be a positive value");
        }
    }
}
