using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain.Model
{
     [Keyless]
    public class TraceProduct
    {
         public string SerialNumber { get; set; }  
        public string StationName { get; set; }
        public string Status { get; set; }
    }
}