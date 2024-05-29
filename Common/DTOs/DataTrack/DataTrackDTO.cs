using Common.DTOs.User;
using Common.DTOs.LastStationID;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.DTOs
{
    public class DataTrackDTO
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
        public string ApprovalId { get; set; }
        public UserDataDto User { get; set; }
        public List<DataTrackCheckingDTO> DataTrackCheckings { get; set; }
        public List<ParameterCheckDTO> ParameterChecks { get; set; }
        public List<ParameterCheckErrorMessageDto> ParameterCheckErrorMessages { get; set; }
        public bool DTisDeleted { get; set; }
    }
}