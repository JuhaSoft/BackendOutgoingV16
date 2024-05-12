using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class DataReferenceParameterCheck
    {
        public Guid Id { get; set; }
   public Guid DataReferenceId { get; set; }
        public DataReference DataReference { get; set; }
        public Guid ParameterCheckId { get; set; }
        public ParameterCheck ParameterCheck { get; set; }
        public int Order { get; set; }
    }
}