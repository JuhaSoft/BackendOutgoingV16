using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class DataLine
    {
        public Guid Id { get; set; }
        public string LineId { get; set; }  
        public string LineName { get; set; }
        public bool isDeleted { get; set; }
        // Navigation property untuk relasi satu-ke-banyak
        // public ICollection<DataReference> DataReferences { get; set; }
    }
}