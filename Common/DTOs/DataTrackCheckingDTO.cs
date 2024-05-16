using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.DTOs.User;

namespace Common.DTOs
{
    public class DataTrackCheckingDTO
    {

       //Cara agar tidak auto increment karena jika ada id otomatis dijadikan Primari key dan otomatis auto increment
        // [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public Guid Id { get; set; }
        public Guid DataTrackID { get; set; }
        public Guid  PCID { get; set; }
        public ICollection<ParameterCheckErrorMessageDto> ParameterCheckErrorMessage { get; set; }

        public string DTCValue { get; set; }
        public Guid ErrorId { get; set; }
        public ErrorMessageDTO? ErrorMessage { get; set; } // Ubah menjadi nullable
        public bool Approve { get; set; }
         public string ApprovalId { get; set; } // ID navigasi ke AppUser untuk pemberi persetujuan
        public UserDto Approver { get; set; }
        public string ApprRemaks { get; set; }
        public ParameterCheckDTO? ParameterCheck { get; set; } // Ubah menjadi nullable
        public DataTrackDTO DataTracks { get; set; }
        // Properti navigasi ke ImageDataCheck
        public ICollection<ImageDataCheckDTO> ImageDataChecks { get; set; }
        public bool DTCisDeleted { get; set; }
    }
}