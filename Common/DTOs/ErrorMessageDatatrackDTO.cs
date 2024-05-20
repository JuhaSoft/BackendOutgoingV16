using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class ErrorMessageDatatrackDTO
    {
         public Guid Id { get; set; }
        public string TrackPSN { get; set; }
        public int Count { get; set; }
        public DateTime TrackingDateCreate { get; set; }
        public Guid PCID { get; set; }
        public Guid? ErrorId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public List<ErrorTrackDTO> ErrorTracks { get; set; }
    }
}