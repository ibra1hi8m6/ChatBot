using AutoMapper;
using ChatBot.Data.AppDbContext;
using ChatBot.Data.Entities;
using ChatBot.Data.ViewModel.MessageViewModel;
using ChatBot.Services.IServices.IChatBotServices;
using ChatBot.Services.IServices.IModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Services.Services.ChatBotServices
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<Agent> _agentRepository;
        private readonly IRepository<FileAttachment> _fileRepository;
        private readonly IModelProvider _modelProvider;
        private readonly IMapper _mapper;

        public MessageService(IRepository<Message> messageRepository, IRepository<Agent> agentRepository, IRepository<FileAttachment> fileRepository,
                          IModelProvider modelProvider, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _agentRepository = agentRepository;
            _fileRepository = fileRepository;
            _modelProvider = modelProvider;
            _mapper = mapper;
        }

        public async Task<SendMessageResultDto> CreateMessageAsync(string userId, CreateMessageDto dto, List<IFormFile>? files = null)
        {
            // 1) ensure agent exists
            var agent = await _agentRepository.GetByIdAsync(dto.AgentId);
            if (agent == null) throw new Exception("Agent not found");

            // Optionally: check user is owner or member
            if (agent.UserId != userId)
            {
                // if you support joinable agents, check membership instead.
                // For now, require owner:
                throw new UnauthorizedAccessException("You are not allowed to send messages to this agent.");
            }

            // 2) Save user message
            var userMessage = new Message
            {
                AgentId = dto.AgentId,
                
                MessageUser = dto.MessageUser,
                CreatedAt = DateTime.UtcNow
            };

            await _messageRepository.AddAsync(userMessage);
            await _messageRepository.SaveAsync();

            // 3) Save attached files (if any), linked to the userMessage
            if (files != null && files.Any())
            {
                // base uploads directory (you can change to webroot or a configurable path)
                var baseUploads = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", userId);
                Directory.CreateDirectory(baseUploads);

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0) continue;

                    var safeFileName = Path.GetFileName(file.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(safeFileName)}";
                    var agentFolder = Path.Combine(baseUploads, $"agent_{dto.AgentId}");
                    Directory.CreateDirectory(agentFolder);
                    var filePath = Path.Combine(agentFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var fa = new FileAttachment
                    {
                        MessageId = userMessage.Id,
                        FileName = safeFileName,
                        FilePath = filePath,
                        FileSize = file.Length
                    };

                    await _fileRepository.AddAsync(fa);
                }

                // save file attachments
                await _fileRepository.SaveAsync();
            }

            // 4) Build context: system prompt + last 10 messages (including this new user message)
            // include attachments if you want to add file names to context
            var allMessages = (await _messageRepository.GetAllAsync(m => m.AgentId == dto.AgentId,
                         q => q.Include(m => m.Attachments)))
                              .OrderBy(m => m.CreatedAt)
                              .ToList();

            var contextMessages = allMessages
                .Select(m =>
                {
                    var role = !string.IsNullOrWhiteSpace(m.MessageAgent) ? "assistant" : "user";
                    var content = (!string.IsNullOrWhiteSpace(m.MessageAgent) ? m.MessageAgent : m.MessageUser) ?? "";
                    // append file names if exist
                    if (m.Attachments != null && m.Attachments.Any())
                    {
                        var names = string.Join(", ", m.Attachments.Select(a => a.FileName));
                        content += $"\n[Attachments: {names}]";
                    }
                    return (Role: role, Content: content);
                })
                .TakeLast(10)
                .ToList();

            string systemPrompt = agent.Prompt ?? "";

            // 5) Call model (wrap in try/catch to handle provider failures)
            string assistantText;
            try
            {
                var modelName = agent.Model ?? "facebook/blenderbot-400M-distill"; // default model from appsettings
                assistantText = await _modelProvider.GetCompletionAsync(systemPrompt, contextMessages, modelName);
            }
            catch (Exception ex)
            {
                assistantText = $"[Error calling model provider: {ex.Message}]";
            }

            // 6) Save assistant message
            var assistantMessage = new Message
            {
                AgentId = dto.AgentId,
                MessageAgent = assistantText,
                CreatedAt = DateTime.UtcNow
            };

            await _messageRepository.AddAsync(assistantMessage);
            await _messageRepository.SaveAsync();

            // 7) Return assistant message DTO
            // fetch assistant message with attachments (none usually) to map properly
            var savedAssistant = await _messageRepository.GetByIdAsync(assistantMessage.Id);
            // optionally include attachments if your repository supports include by GetByIdAsync - if not, map manually
            var userDto = _mapper.Map<MessageDto>(userMessage);
            var assistantDto = _mapper.Map<MessageDto>(assistantMessage);

            return new SendMessageResultDto
            {
                UserMessage = userDto,
                AssistantMessage = assistantDto
            };
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesByAgentAsync(int agentId, string userId)
        {
            // verify user owns agent
            var agent = await _agentRepository.GetByIdAsync(agentId);
            if (agent == null) throw new Exception("Agent not found");
            if (agent.UserId != userId) throw new UnauthorizedAccessException("Not allowed.");

            var messages = await _messageRepository.GetAllAsync(m => m.AgentId == agentId,
                q => q.Include(m => m.Attachments));

            // map and return ordered
            return _mapper.Map<IEnumerable<MessageDto>>(messages.OrderBy(m => m.CreatedAt));
        }
    }
}
