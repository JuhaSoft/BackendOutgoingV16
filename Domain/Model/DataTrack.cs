using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class DataTrack
    {
        //Cara agar tidak auto increment karena jika ada id otomatis dijadikan Primari key dan otomatis auto increment
        // [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public Guid Id { get; set; }
        public string TrackPSN { get; set; }
        public string TrackReference { get; set; }
        public string TrackingWO { get; set; }
        public Guid TrackingLastStationId { get; set; }
        public LastStationID LastStationID { get; set; }
        // DataInsert akan otomatis diatur ketika data disisipkan
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime TrackingDateCreate { get; set; }
        public string TrackingResult { get; set; }
        public string TrackingStatus { get; set; }
          public string TrackingUserIdChecked { get; set; }
        public AppUser User { get; set; }
        public List< DataTrackChecking> DataTrackCheckings { get; set; }
        public bool DTisDeleted { get; set; }
        
    }
}