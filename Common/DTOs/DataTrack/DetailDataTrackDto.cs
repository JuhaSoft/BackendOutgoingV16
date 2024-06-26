using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Common.DTOs.User;
using Common.DTOs.LastStationID;

namespace Common.DTOs.DataTrack
{
    public class DetailDataTrackDto
    {
        public Guid Id { get; set; }
        public string TrackPSN { get; set; }
        public string TrackReference { get; set; }
        public string TrackingWO { get; set; }
        public Guid TrackingLastStationId { get; set; }
        public LastStationIDDTO LastStationID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime TrackingDateCreate { get; set; }
        public string TrackingResult { get; set; }
        public string TrackingStatus { get; set; }
        public Guid TrackingUserIdChecked { get; set; }
        public UserDataDto User { get; set; }
        public List<DataTrackCheckingDTO> DataTrackCheckings { get; set; }
        public bool DTisDeleted { get; set; }
    }
}