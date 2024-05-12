using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class ErrorMessageDTO
    {
         public Guid Id { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
         public ICollection<ParameterCheckErrorMessageDto> ParameterCheckErrorMessages { get; set; }
    }
}