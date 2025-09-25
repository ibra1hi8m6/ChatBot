using ChotBot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Data.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public Agent Agent { get; set; }

        

        public string? MessageUser { get; set; }
        public string? MessageAgent { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<FileAttachment> Attachments { get; set; } = new List<FileAttachment>();
    }
}
