using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ErrorMessage
    {
        public Guid Id { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
         public ICollection<ParameterCheckErrorMessage> ParameterCheckErrorMessages { get; set; }
    }
}