using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs.LastStationID
{
    public class LastStationIDDTO
    {
                public Guid Id { get; set; }
        public string StationID { get; set; }
        public string StationName { get; set; }

        public Guid LineId { get; set; }  
        public  DataLineDTO DataLine { get; set; }
    }
}