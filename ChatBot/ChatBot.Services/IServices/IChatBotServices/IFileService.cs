using ChatBot.Data.ViewModel.FAViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Services.IServices.IChatBotServices
{

    public interface IFileService
    {
        Task<FileAttachmentDto> AddFileAsync(int messageId, CreateFileAttachmentDto dto);
    }
}
