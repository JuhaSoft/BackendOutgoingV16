using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.DTOs.User;

namespace Common.DTOs
{
    public class WorkOrderDto
    {
         public Guid Id { get; set; }
        public string WoNumber { get; set; }
        public string SONumber { get; set; }
        public string WoReferenceID { get; set; }
        public string WoQTY { get; set; }
        public string PassQTY { get; set; }
        public string FailQTY { get; set; }
        public string WoStatus { get; set; }
        public DateTime WoCreate { get; set; }
        public string UserIdCreate { get; set; }
         public UserDataDto User { get; set; }
        public bool WOisDeleted { get; set; }
        
    }
}