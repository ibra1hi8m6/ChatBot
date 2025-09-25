using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Data.ViewModel.FAViewModel
{
    public class CreateFileAttachmentDto
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long? FileSize { get; set; }
    }
}
