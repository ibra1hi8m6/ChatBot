using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Data.ViewModel.AgentViewModel
{

    public class CreateAgentDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Prompt { get; set; }
    }
}
