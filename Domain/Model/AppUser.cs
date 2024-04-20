using System;
using System.Collections.Generic;
using Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Domain.Model
{
    public class AppUser : IdentityUser
    {

        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}