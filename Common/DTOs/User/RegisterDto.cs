using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs.User
{
    public class RegisterDto
    {
     [Required]
        public string DisplayName { get; set; }
        [Required]

        public string Username { get; set; }
        [Required]

        public string Password { get; set; }
        
        
        
        
        
        
    }
}