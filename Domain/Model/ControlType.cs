using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public enum DataControl{
        Combo,
        Select,
        SelectOption,
        Data
    }
    public class ControlType
    {
        //Cara agar tidak auto increment karena jika ada id otomatis dijadikan Primari key dan otomatis auto increment
        // [DatabaseGenerated(DatabaseGeneratedOption.None)] 
     public Guid Id { get; set; }
     public DataControl  CTName { get; set; }
     public bool isDeleted { get; set; }
    }
}