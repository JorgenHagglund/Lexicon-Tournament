using AutoMapper;
using Tournament.Core.DTOs;
using Tournament.Core.Entities;

namespace Tournament.Data.Data;

public class TournamentMappings : Profile
{
    public TournamentMappings()
    {
        CreateMap<Core.Entities.Tournament, TournamentDto>().ReverseMap();
        CreateMap<Core.Entities.Tournament, TournamentUpdateDto>().ReverseMap();
        CreateMap<Game, GameDto>().ReverseMap();
        //.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Time))
        //.ReverseMap();

        CreateMap<GameToCreateDto, Game>();
        CreateMap<Game, GameUpdateDto>().ReverseMap();
    }
}
