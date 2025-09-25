using ChatBot.Data.ViewModel.AgentViewModel;
using ChatBot.Services.IServices.IChatBotServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatBot.APIs.Controllers.ChatBotControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AgentsController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [HttpPost ("CreateAgent")]
        public async Task<IActionResult> CreateAgent([FromBody] CreateAgentDto dto)
        {
            var userId = User.FindFirstValue("UserID");
            var agent = await _agentService.CreateAgentAsync(userId, dto);
            return Ok(agent);
        }

        [HttpGet("GetMyAgents")]
        public async Task<IActionResult> GetMyAgents()
        {
            var userId = User.FindFirstValue("UserID");
            var agents = await _agentService.GetAgentsByUserAsync(userId);
            return Ok(agents);
        }

    }
}
