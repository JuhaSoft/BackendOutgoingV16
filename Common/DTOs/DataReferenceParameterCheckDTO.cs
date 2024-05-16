using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class DataReferenceParameterCheckDTO
    {
       public Guid Id { get; set; }
   public Guid DataReferenceId { get; set; }
        public DataReferenceDTO DataReference { get; set; }
        public Guid ParameterCheckId { get; set; }
        public ParameterCheckDTO ParameterCheck { get; set; }
        public int Order { get; set; } 
    }
}