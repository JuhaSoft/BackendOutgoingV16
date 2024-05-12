using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class ParameterCheckErrorMessageDto
    {
        public Guid Id { get; set; }
        public Guid ParameterCheckId { get; set; }
        public ParameterCheckDTO ParameterCheck { get; set; }
        public Guid ErrorMessageId { get; set; }
        public ErrorMessageDTO ErrorMessage { get; set; }
        public int Order { get; set; }
    }
}