using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class SComboBoxOption
    {
        //Cara agar tidak auto increment karena jika ada id otomatis dijadikan Primari key dan otomatis auto increment
        // [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public Guid Id { get; set; }
        public string PCID { get; set; }
        public string OptionValue { get; set; }
    }
}