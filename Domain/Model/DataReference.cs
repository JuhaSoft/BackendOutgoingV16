using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
   	public class DataReference
    {
        public Guid Id { get; set; }
        public string RefereceName { get; set; }
        public string PsnPos { get; set; }
        public string RefPos { get; set; }
        public string RefCompare { get; set; }

        public Guid StationID { get; set; }
        public bool isDeleted { get; set; }

        // Navigation property untuk relasi satu-ke-banyak
        public LastStationID LastStationID { get; set; }
        //public List<ParameterCheck> ParameterChecks { get; set; }
    }
}