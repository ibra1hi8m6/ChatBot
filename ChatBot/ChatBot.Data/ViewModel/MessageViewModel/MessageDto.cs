using ChatBot.Data.ViewModel.FAViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Data.ViewModel.MessageViewModel
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string? MessageUser { get; set; }
        public string? MessageAgent { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<FileAttachmentDto> Attachments { get; set; } = new();
    }
}
