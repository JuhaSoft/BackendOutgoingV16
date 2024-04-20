using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ParameterCheck
    {
    public Guid Id { get; set; }
    public string Description { get; set; }
    public int Order { get; set; } // Urutan deskripsi di dalam grup reference

    // Relasi ke tabel DataReference
    public Guid DataReferenceId { get; set; }
    public DataReference DataReference { get; set; }
    }
}