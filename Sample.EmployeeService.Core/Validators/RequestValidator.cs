using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Core.Validators
{
    public class RequestValidator<T>
    {
        public T Value { get; set; }
        public bool IsValid { get; set; }
        public IList<ValidationFailure> Errors { get; set; }
        public IDictionary<string, string> CustomErrors { get; set; }
    }
}
