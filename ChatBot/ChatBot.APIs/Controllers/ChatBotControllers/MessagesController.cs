using ChatBot.Data.ViewModel.MessageViewModel;
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
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        [RequestSizeLimit(50_000_000)] // optional limit
        public async Task<IActionResult> Send([FromForm] SendMessageFormModel model)
        {
            var userId = User.FindFirstValue("UserID");
            var createDto = new CreateMessageDto
            {
                AgentId = model.AgentId,
                MessageUser = model.MessageUser
            };

            var result = await _messageService.CreateMessageAsync(userId, createDto, model.Files);
            return Ok(result);
        }


        [HttpGet("{agentId}")]
        public async Task<IActionResult> GetMessagesByAgent(int agentId)
        {
            var userId = User.FindFirstValue("UserID");
            var messages = await _messageService.GetMessagesByAgentAsync(agentId, userId);
            return Ok(messages);
        }
    }
}
