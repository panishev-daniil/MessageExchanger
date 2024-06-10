using AutoMapper;
using MessageExchanger.Abstractions.Models;
using MessageExchanger.WEB.Dtos;

namespace MessageExchanger.WEB
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Message, CreateMessageDto>()
                .ReverseMap();
            CreateMap<Message, MessageDto>()
                .ReverseMap();
        }
    }
}
