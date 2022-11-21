using AutoMapper;
using GymTracker.Dto;
using GymTracker.Entities;

namespace GymTracker.AutoMapperProfiles;

public class SessionProfile : Profile
{
    public SessionProfile()
    {
        CreateMap<Session, SessionDto>()
            .ForMember(dest => dest.NbExercises,
                opt => opt.MapFrom(src => src.Exercises.Count))
            .ForMember(dest => dest.Duration,
                opt => opt.MapFrom(src => src.EndAt - src.StartAt));
    }
}