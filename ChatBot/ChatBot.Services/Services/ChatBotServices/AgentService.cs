using AutoMapper;
using ChatBot.Data.AppDbContext;
using ChatBot.Data.Entities;
using ChatBot.Data.ViewModel.AgentViewModel;
using ChatBot.Services.IServices.IChatBotServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Services.Services.ChatBotServices
{
    public class AgentService : IAgentService
    {
        private readonly IRepository<Agent> _agentRepository;
        private readonly IMapper _mapper;

        public AgentService(IRepository<Agent> agentRepository, IMapper mapper)
        {
            _agentRepository = agentRepository;
            _mapper = mapper;
        }

        public async Task<AgentDto> CreateAgentAsync(string userId, CreateAgentDto dto)
        {
            var agent = new Agent
            {
                UserId = userId,
                Name = dto.Name,
                Description = dto.Description,
                Prompt = dto.Prompt
            };

            await _agentRepository.AddAsync(agent);
            await _agentRepository.SaveAsync();

            return _mapper.Map<AgentDto>(agent);
        }

        public async Task<IEnumerable<AgentDto>> GetAgentsByUserAsync(string userId)
        {
            var agents = await _agentRepository.GetAllAsync(a => a.UserId == userId);
            return _mapper.Map<IEnumerable<AgentDto>>(agents);
        }

        public async Task<AgentDto?> GetAgentByIdAsync(int id, string userId)
        {
            var agent = await _agentRepository.GetByIdAsync(id);
            if (agent == null || agent.UserId != userId) return null;

            return _mapper.Map<AgentDto>(agent);
        }
    }
}
