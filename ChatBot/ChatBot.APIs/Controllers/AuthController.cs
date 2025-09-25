using ChatBot.Data.ViewModel.UserViewModel;
using ChatBot.Services.IServices.IUserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTOs model)
        {
            var user = await _userService.RegisterAsync(model);
            var response = new RegisterResponse
            {
                Successed = true,
                Message = "User registered successfully"
            };

            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTOs model)
        {
            return await _userService.LoginAsync(model);
        }
    }
}
