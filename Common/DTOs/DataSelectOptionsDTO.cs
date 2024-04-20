using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class DataSelectOptionsDTO
    {
        //Cara agar tidak auto increment karena jika ada id otomatis dijadikan Primari key dan otomatis auto increment
        // [DatabaseGenerated(DatabaseGeneratedOption.None)] 
         public Guid Id { get; set; }
        public string PCID { get; set; }
        public string SOptionValue { get; set; }
    }
}