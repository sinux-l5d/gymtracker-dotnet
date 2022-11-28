namespace GymTracker.Dto;

public class AddExerciseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Repetitions { get; set; }
}