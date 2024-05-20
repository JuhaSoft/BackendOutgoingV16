using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs.DataTrack
{
    public class DataTrackGrapDTO
    {
        public Guid Id { get; set; }
        
        public DateTime TrackingDateCreate { get; set; }
        public string TrackingResult { get; set; }
        public string TrackingStatus { get; set; }
         
        public bool DTisDeleted { get; set; }
    }
}