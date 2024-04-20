using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ImageDataCheck
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } // Kolom untuk menyimpan URL gambar
        // Properti navigasi ke DataTrackChecking
        public Guid DataTrackCheckingId { get; set; }
        public DataTrackChecking DataTrackChecking { get; set; }
    }
}