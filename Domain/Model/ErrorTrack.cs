using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ErrorTrack
    {
        public Guid Id { get; set; }
        public string TrackPSN { get; set; } 
        public DateTime TrackingDateCreate { get; set; }
        public Guid  PCID { get; set; }
        public ParameterCheck ParameterCheck { get; set; }
        public Guid? ErrorId { get; set; }
        public ErrorMessage ErrorMessage { get; set; }

    }
}