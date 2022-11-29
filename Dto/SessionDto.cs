namespace GymTracker.Dto;

public class SessionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public TimeSpan Duration { get; set; }
    public int NbExercises { get; set; }
}