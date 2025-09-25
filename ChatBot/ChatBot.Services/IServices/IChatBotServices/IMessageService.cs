using ChatBot.Data.ViewModel.MessageViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Services.IServices.IChatBotServices
{
    public interface IMessageService
    {
        Task<SendMessageResultDto> CreateMessageAsync(string userId, CreateMessageDto dto, List<IFormFile>? files = null);
        Task<IEnumerable<MessageDto>> GetMessagesByAgentAsync(int agentId, string userId);
    }
}
