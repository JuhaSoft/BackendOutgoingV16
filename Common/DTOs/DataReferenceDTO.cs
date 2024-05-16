using Common.DTOs.LastStationID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
   	public class DataReferenceDTO
    {
        public Guid Id { get; set; }
        public string RefereceName { get; set; }
        public Guid StationID { get; set; }
        public bool isDeleted { get; set; }
        public string PsnPos { get; set; }
        public string RefPos { get; set; }
        public string RefCompare { get; set; }
        // Navigation property untuk relasi satu-ke-banyak
        public LastStationIDDTO LastStation { get; set; }
        //public List<ParameterCheckDTO> ParameterChecks { get; set; }
        public ICollection<DataReferenceParameterCheckDTO> DataReferenceParameterChecks { get; set; }
    }
}