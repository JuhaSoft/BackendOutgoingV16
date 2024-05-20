using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class ErrorTrackChartDTO
    {
        public string TrackPSN { get; set; }
        public DateTime TrackingDateCreate { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public Guid? ErrorId { get; set; }
        public ErrorMessageDTO ErrorMessage { get; set; }
    }
}