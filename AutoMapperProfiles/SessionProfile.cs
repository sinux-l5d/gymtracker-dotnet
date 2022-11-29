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
                opt => opt.MapFrom(src => src.Exercises.Count()))
            .ForMember(dest => dest.Duration,
                opt => opt.MapFrom(src => src.EndAt - src.StartAt));
        CreateMap<UpdateSessionDto, Session>()
            .ForMember(dest => dest.EndAt,
                opt => opt.MapFrom(src => src.StartAt!.Value.Add(src.Duration!.Value)));
        ;

        // If AddSessionDto.StartAt is empty, it will be set to DateTime.Now
        // If EndAt is empty, it will be set to StartAt + Duration
        // If Duration is empty and EndAt is empty, it will be set to 1 hour
        CreateMap<AddSessionDto, Session>()
            .ForMember(dest => dest.StartAt,
                opt => { opt.MapFrom(src => src.StartAt ?? DateTime.Now); })
            .ForMember(dest => dest.EndAt,
                opt =>
                {
                    opt.MapFrom(src =>
                        // if EndAt is not null, return EndAt
                        // if EndAt is null, return StartAt + Duration
                        // Default value for StartAt is DateTime.Now
                        // Default value for Duration is 1 hour
                        // Not using StartAt directly because we don't know the mapping order
                        src.EndAt ?? (src.StartAt ?? DateTime.Now).Add(src.Duration ?? TimeSpan.FromHours(1.0))
                    );
                });
    }
}