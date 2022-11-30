namespace GymTracker.Dto;

public class UpdateSessionDto
{
    public string? Name { get; set; }
    public string? Location { get; set; }
    public DateTime? StartAt { get; set; }
    public TimeSpan? Duration { get; set; }
}