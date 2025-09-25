using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ChatBot.Services.IServices.IUserServices;
using ChatBot.Data.ViewModel.UserViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ChotBot.Data.Entities;

namespace ChatBot.Services.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<ApplicationUser> RegisterAsync(RegisterDTOs model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                PhoneNumber = model.PhoneNumber,
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            
            return user;
        }

        public async Task<IActionResult> LoginAsync(LoginDTOs model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.SecondName}"),
                        
                        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? "")
                    });
                    var tokenDescription = new SecurityTokenDescriptor
                {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddMinutes(30),
                        Issuer = _config["Jwt:Issuer"],     // ✅ Add Issuer
                        Audience = _config["Jwt:Audience"], // ✅ Add Audience
                        SigningCredentials = new SigningCredentials(
                        key,
                        SecurityAlgorithms.HmacSha256Signature
                        )
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescription);
                var token = tokenHandler.WriteToken(securityToken);
                return new OkObjectResult(new { token });
            }
            else
            {
                 return new BadRequestObjectResult(new { Message = "Username or password is incorrect" });
            }
               
        }
       

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        
    }
}
