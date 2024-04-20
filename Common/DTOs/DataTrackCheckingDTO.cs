using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class DataTrackCheckingDTO
    {

       //Cara agar tidak auto increment karena jika ada id otomatis dijadikan Primari key dan otomatis auto increment
        // [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public Guid Id { get; set; }
        public Guid DataTrackID { get; set; }
        public Guid  PCID { get; set; }
        public string DTCValue { get; set; }
        
        public ParameterCheckDTO ParameterChecks { get; set; }
        public DataTrackDTO DataTracks { get; set; }
        // Properti navigasi ke ImageDataCheck
        public ICollection<ImageDataCheckDTO> ImageDataChecks { get; set; }
        public bool DTCisDeleted { get; set; }
    }
}