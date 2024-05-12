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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png; // Import PngEncoder

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
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService, IWebHostEnvironment hostingEnvironment)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
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
        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Name));
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to change password.");
            }

            return Ok("Password changed successfully.");
        }
        [AllowAnonymous]
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
                Email=registerDto.Email
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
                Email = user.Email,
                Image = user.Image,
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
            user.Email = updateDto.Email;
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
        public async Task<ActionResult<UserDto>> UpdateProfile([FromForm] UserUpdateDto updateDto, IFormFile image)
        {
            var user = await _userManager.FindByNameAsync(updateDto.UserName);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Jika user mengirimkan gambar baru
            if (image != null)
            {
                try
                {
                    var userName = user.UserName ?? "";
                    var uploadsPath = Path.Combine("Upload", "Image", "User", "profil", userName);

                    // Full path to wwwroot
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    if (string.IsNullOrEmpty(webRootPath))
                    {
                        // Jika webRootPath kosong, gunakan folder default
                        webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        Console.WriteLine("WebRootPath is null or empty. Using default folder.");
                    }

                    var uploadsFolder = Path.Combine(webRootPath, uploadsPath);

                    // Jika folder Upload/Image/User/profil belum ada, buat folder
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var imageStream = image.OpenReadStream())
                    {
                        // Load gambar menggunakan ImageSharp
                        using (var imageSharp = Image.Load(imageStream))
                        {
                            // Resize gambar menjadi 200x200 (contoh)
                            imageSharp.Mutate(x => x.Resize(200, 200));

                            // Simpan gambar yang sudah diresize
                            var extension = Path.GetExtension(image.FileName).ToLower();
                            var uniqueFileName = Guid.NewGuid().ToString() + extension;
                            var imagePath = Path.Combine(uploadsFolder, uniqueFileName);

                            // Menentukan encoder (PNG dalam contoh ini)
                            var pngEncoder = new PngEncoder();

                            using (var outputStream = new FileStream(imagePath, FileMode.Create))
                            {
                                // Simpan gambar yang sudah diresize ke server
                                imageSharp.Save(outputStream, pngEncoder);
                            }

                            // Mengupdate path gambar dalam data user
                            user.Image = $"/{uploadsPath}/{uniqueFileName}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Tangkap dan cetak pesan error jika terjadi masalah
                    Console.WriteLine($"Error: {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }

            // Update properti user
            user.UserName = updateDto.UserName;
            user.DisplayName = updateDto.DisplayName;

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
                Email = user.Email,
                Image = user.Image,
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
                Image = user.Image,
                UserName = user.UserName,
                Email = user.Email,
                IsActive =user.IsActive,
                Role = user.Role
            };
        }
    }
}