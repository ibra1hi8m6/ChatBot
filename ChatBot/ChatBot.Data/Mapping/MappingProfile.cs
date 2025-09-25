using AutoMapper;
using ChatBot.Data.Entities;
using ChatBot.Data.ViewModel.AgentViewModel;
using ChatBot.Data.ViewModel.FAViewModel;
using ChatBot.Data.ViewModel.MessageViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatBot.Data.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Agent, AgentDto>().ReverseMap();
            CreateMap<CreateAgentDto, Agent>();

            CreateMap<Message, MessageDto>()
            .ForMember(d => d.Attachments, opt => opt.MapFrom(s => s.Attachments));
            CreateMap<CreateMessageDto, Message>();

            CreateMap<FileAttachment, FileAttachmentDto>().ReverseMap();
            CreateMap<CreateFileAttachmentDto, FileAttachment>();
        }
    }
}
