using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Data.ViewModel.MessageViewModel
{
    public class SendMessageResultDto
    {
        public MessageDto UserMessage { get; set; }
        public MessageDto AssistantMessage { get; set; }
    }
}
