namespace GymTracker.Dto;

public class ExerciseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Repetitions { get; set; }
}