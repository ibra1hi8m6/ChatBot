using ChotBot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Data.Entities
{
    public class Agent
    {
        public int Id { get; set; }  
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Prompt { get; set; }
        public string Model { get; set; } = "facebook/blenderbot-400M-distill";
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }

}
