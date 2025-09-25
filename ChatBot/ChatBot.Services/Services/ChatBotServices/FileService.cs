using AutoMapper;
using ChatBot.Data.AppDbContext;
using ChatBot.Data.Entities;
using ChatBot.Data.ViewModel.FAViewModel;
using ChatBot.Services.IServices.IChatBotServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Services.Services.ChatBotServices
{
    public class FileService : IFileService
    {
        private readonly IRepository<FileAttachment> _fileRepository;
        private readonly IMapper _mapper;

        public FileService(IRepository<FileAttachment> fileRepository, IMapper mapper)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        public async Task<FileAttachmentDto> AddFileAsync(int messageId, CreateFileAttachmentDto dto)
        {
            var file = new FileAttachment
            {
                MessageId = messageId,
                FileName = dto.FileName,
                FilePath = dto.FilePath,
                FileSize = dto.FileSize
            };

            await _fileRepository.AddAsync(file);
            await _fileRepository.SaveAsync();

            return _mapper.Map<FileAttachmentDto>(file);
        }
    }
}
