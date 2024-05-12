using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ParameterCheckErrorMessage
    {
        public Guid Id { get; set; }
        public Guid ParameterCheckId { get; set; }
        public ParameterCheck ParameterCheck { get; set; }
        public Guid ErrorMessageId { get; set; }
        public ErrorMessage ErrorMessage { get; set; }
        public int Order { get; set; }
    }
}