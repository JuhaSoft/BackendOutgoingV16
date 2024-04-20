using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class WorkOrder
    {
        public Guid Id { get; set; }
        [Key]
        // [Required]
        public string WoNumber { get; set; }
        // [Required]

        public string SONumber { get; set; }
        // [Required]

        public string WoReferenceID { get; set; }
        // [Required]
        public string WoQTY { get; set; }
        public string WoStatus { get; set; }
        // DataInsert akan otomatis diatur ketika data disisipkan
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //  [DefaultValue("getdate()")]
    // [Column(TypeName = "datetime2")]
        public DateTime WoCreate { get; set; }
        // [Required]
public string UserIdCreate { get; set; }
        public AppUser User { get; set; }
                public bool WOisDeleted { get; set; }
    }
}