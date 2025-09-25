using ChatBot.Data.ViewModel.UserViewModel;
using ChotBot.Data.Entities;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Services.IServices.IUserServices
{
    public interface IUserService
    {
        Task<ApplicationUser> RegisterAsync(RegisterDTOs model);
        Task<IActionResult> LoginAsync(LoginDTOs model);
    
        Task LogoutAsync();
        
    }
}
