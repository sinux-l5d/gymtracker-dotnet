using AutoMapper;
using GymTracker.Dto;
using GymTracker.Entities;

namespace GymTracker.AutoMapperProfiles;

public class ExerciseProfile : Profile
{
    public ExerciseProfile()
    {
        CreateMap<AddExerciseDto, Exercise>();
        CreateMap<Exercise, ExerciseDto>();
    }
}