using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class WebConfigData
    {
        public Guid Id { get; set; }
        public string WebTitle { get; set; }
        public string WebDescription { get; set; }
        public string EmailRegisterTitle { get; set; }
        public string EmailRegisterBody { get; set; }
        public string EmailInfoTitle { get; set; }
        public string EmailInfoBody { get; set; }
    }
}