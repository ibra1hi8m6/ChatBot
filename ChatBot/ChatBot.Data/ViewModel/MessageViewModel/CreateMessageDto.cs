using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Data.ViewModel.MessageViewModel
{
    public class CreateMessageDto
    {
        public int AgentId { get; set; }
        public string? MessageUser { get; set; }
    }
}
