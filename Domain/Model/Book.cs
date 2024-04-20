using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Book
    {
         public int BookId { get; set; }
            public string Title { get; set; }
            public int AuthorId { get; set; } // Kunci luar untuk menunjukkan penulis dari buku ini
            public Author Author { get; set; } // Properti navigasi untuk menyimpan informasi penulis buku ini
    }
}