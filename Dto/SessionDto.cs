namespace GymTracker.Dto;

public class SessionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public DateTime StartAt { get; set; }
    public TimeSpan Duration { get; set; }
    public int NbExercises { get; set; }
}