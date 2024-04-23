using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Image { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; } 
    }
}