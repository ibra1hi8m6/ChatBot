using ChatBot.Data.ViewModel.AgentViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Services.IServices.IChatBotServices
{
    public interface IAgentService
    {
        Task<AgentDto> CreateAgentAsync(string userId, CreateAgentDto dto);
        Task<IEnumerable<AgentDto>> GetAgentsByUserAsync(string userId);
        Task<AgentDto?> GetAgentByIdAsync(int id, string userId);
    }
}
