using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.DTOs.User;
using API.Services;
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    //[AllowAnonymous]
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;

        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null) return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }
            return Unauthorized();
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                return BadRequest("Username is already taken.");
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Username,
                Role = registerDto.Role,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, user.Role);

                if (!roleResult.Succeeded)
                {
                    return BadRequest("Failed to assign role to user.");
                }

                return CreateUserObject(user);
            }

            return BadRequest(result.Errors.Select(e => e.Description).ToList());
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Name));
            return CreateUserObject(user);
        }
        [Authorize]
        [HttpPost("refreshToken")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await _userManager.Users
                .Include(r => r.RefreshTokens)
                .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

            if (user == null) return Unauthorized();

            var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

            if (oldToken != null && !oldToken.IsActive) return Unauthorized();

            return CreateUserObject(user);
        }

        private async Task SetRefreshToken(AppUser user)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
        [AuthorizeRoles("Admin,Staf")]
        //[AuthorizeRoles("Admin,Staff")]
        [HttpGet("allUsers")]
        public async Task<ActionResult<IEnumerable<UserDataDto>>> GetAllUsers()
        {
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            var users = await _userManager.Users.ToListAsync();

            var userDtos = users.Select(user => new UserDto
            {
                Id = Guid.Parse(user.Id),
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Image = null,
                Role = user.Role,
                IsActive = user.IsActive // Properti IsActive
            }).ToList();

            return Ok(userDtos);
        }

        [AuthorizeRoles("Admin,Staf")]
        [HttpPut("update")]
        public async Task<ActionResult<UserDto>> UpdateUser(UserUpdateDto updateDto)
        {
            // Hanya admin yang bisa melakukan operasi ini
            if (!User.IsInRole("Admin"))
            {
                return Forbid("Only admin can perform this operation.");
            }

            // Menemukan user berdasarkan UserName
            var user = await _userManager.FindByNameAsync(updateDto.UserName);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update properti user
            user.UserName = updateDto.UserName;
            user.DisplayName = updateDto.DisplayName;
            user.Role = updateDto.Role;
            user.IsActive = updateDto.IsActive;

            // Melakukan update user
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Return updated user
                return CreateUserObject(user);
            }

            // Jika gagal update
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest($"Failed to update user. Errors: {errors}");
        }
        [Authorize]
        [HttpPut("updateProfile")]
        public async Task<ActionResult<UserDto>> UpdateProfile(UserUpdateDto updateDto)
        {
           
            // Menemukan user berdasarkan UserName
            var user = await _userManager.FindByNameAsync(updateDto.UserName);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update properti user
            user.UserName = updateDto.UserName;
            user.DisplayName = updateDto.DisplayName;
            user.Image = updateDto.Image;

            // Melakukan update user
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Return updated user
                return CreateUserObject(user);
            }

            // Jika gagal update
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest($"Failed to update user. Errors: {errors}");
        }

        [AuthorizeRoles("Admin,Staf")]
        //[AllowAnonymous]
        [HttpPut("updateDelete/{Id}")]
        public async Task<ActionResult<UserDto>> UDeleteUser(Guid Id)
        {
            // Hanya admin yang bisa melakukan operasi ini
            if (!User.IsInRole("Admin"))
            {
                return Forbid("Only admin can perform this operation.");
            }

            // Menemukan user berdasarkan UserName
            var user = await _userManager.FindByIdAsync(Id.ToString());

            if (user == null)
            {
                return NotFound("User not found.");
            }


            user.IsActive = false;

            // Melakukan update user
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Return updated user
                return CreateUserObject(user);
            }

            // Jika gagal update
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest($"Failed to Delete user. Errors: {errors}");
        }


        //[AuthorizeRoles("Admin,Staf")]
        //[HttpPut("updateSelf")]
        //public async Task<ActionResult<UserDto>> UpdateUserSelf(UserUpdateDto updateDto)
        //{
        //    // Mendapatkan ID pengguna dari Claim
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    // Menemukan user berdasarkan ID
        //    var user = await _userManager.FindByIdAsync(userId);

        //    if (user == null)
        //    {
        //        return NotFound("User not found.");
        //    }

        //    // Check jika UserName diubah
        //    if (updateDto.UserName != user.UserName)
        //    {
        //        // Jika berbeda, periksa apakah UserName baru sudah digunakan
        //        var existingUser = await _userManager.FindByNameAsync(updateDto.UserName);
        //        if (existingUser != null && existingUser.Id != user.Id)
        //        {
        //            // UserName sudah diambil oleh user lain
        //            return BadRequest("Username is already taken.");
        //        }
        //    }

        //    // Update properti user
        //    user.UserName = updateDto.UserName;
        //    user.DisplayName = updateDto.DisplayName;
        //    // user.Image = updateDto.Image; // Perhatikan: Anda bisa tambahkan ini jika ingin mendukung update gambar
        //    user.Role = updateDto.Role;
        //    user.IsActive = updateDto.IsActive;

        //    // Melakukan update user
        //    var result = await _userManager.UpdateAsync(user);

        //    if (result.Succeeded)
        //    {
        //        // Return updated user
        //        return CreateUserObject(user);
        //    }

        //    // Jika gagal update
        //    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        //    return BadRequest($"Failed to update user. Errors: {errors}");
        //}

        [AuthorizeRoles("Admin,Staf")] // Hanya admin dan staf yang bisa mengakses
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDataDto>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return CreateUserDataObject(user);
        }

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                Id = Guid.Parse(user.Id),
                DisplayName = user.DisplayName,
                Image = null,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName,
                IsActive=user.IsActive,
                Role = user.Role
            };
        }
        private UserDataDto CreateUserDataObject(AppUser user)
        {
            return new UserDataDto
            {
                Id = Guid.Parse(user.Id),
                DisplayName = user.DisplayName,
                Image = null,
                UserName = user.UserName,
                IsActive=user.IsActive,
                Role = user.Role
            };
        }
    }
}