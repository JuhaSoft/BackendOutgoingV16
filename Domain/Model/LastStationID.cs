using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{	public class LastStationID
    {
        public Guid Id { get; set; }
        public string StationID { get; set; }
        public string StationName { get; set; }
        public Guid LineId { get; set; }  
        public  DataLine DataLine { get; set; }
    }
}