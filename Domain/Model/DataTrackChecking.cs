using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class DataTrackChecking
    {
        //Cara agar tidak auto increment karena jika ada id otomatis dijadikan Primari key dan otomatis auto increment
        // [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public Guid Id { get; set; }
        public Guid DataTrackID { get; set; }
        public Guid  PCID { get; set; }
        public string DTCValue { get; set; }
        
        public ParameterCheck ParameterCheck { get; set; }
        public DataTrack DataTracks { get; set; }
        // Properti navigasi ke ImageDataCheck
        public ICollection<ImageDataCheck> ImageDataChecks { get; set; }
        public bool DTCisDeleted { get; set; }
    }
}