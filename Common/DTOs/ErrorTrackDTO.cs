using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class ErrorTrackDTO
    {
        public Guid Id { get; set; }
        public string TrackPSN { get; set; } 
        public DateTime TrackingDateCreate { get; set; }
        public Guid  PCID { get; set; }
        public ParameterCheckDTO ParameterCheck { get; set; }
        public Guid? ErrorId { get; set; }
        public ErrorMessageDTO ErrorMessage { get; set; }
    }
}