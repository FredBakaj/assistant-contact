using AutoMapper;
using AssistantContract.TgBot.Core.Model;
using Telegram.Bot.Types;

namespace AssistantContract.TgBot.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UpdateBDto, Update>().ReverseMap();
        }
    }
}
