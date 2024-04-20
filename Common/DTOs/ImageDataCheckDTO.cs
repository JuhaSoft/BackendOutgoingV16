using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class ImageDataCheckDTO
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } // Kolom untuk menyimpan URL gambar
        // Properti navigasi ke DataTrackChecking
        public Guid DataTrackCheckingId { get; set; }
        public DataTrackCheckingDTO DataTrackChecking { get; set; }
    }
}