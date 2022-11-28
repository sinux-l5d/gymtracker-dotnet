namespace GymTracker.Dto;

public class UpdateSessionDto
{
    public string? Name { get; set; } = string.Empty;
    public string? Location { get; set; } = string.Empty;
    public DateTime? StartAt { get; set; }
    public TimeSpan? Duration { get; set; }
}