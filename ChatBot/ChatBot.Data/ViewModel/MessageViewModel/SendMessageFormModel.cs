using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ChatBot.Data.ViewModel.MessageViewModel
{
    public class SendMessageFormModel
    {
        public int AgentId { get; set; }
        public string MessageUser { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
