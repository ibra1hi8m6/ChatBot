using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Data.ViewModel.FAViewModel
{
    public class FileAttachmentDto
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long? FileSize { get; set; }
    }
}
