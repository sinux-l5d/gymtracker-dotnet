using System.ComponentModel.DataAnnotations;

namespace GymTracker.Dto;

public class AddSessionDto
{
    [MaxLength(100)] public string Name { get; set; } = string.Empty;
    [MaxLength(100)] public string Location { get; set; } = string.Empty;

    // If not provided, the current datetime will be used
    public DateTime? StartAt { get; set; }

    // At least one of these must be set
    public DateTime? EndAt { get; set; }

    public TimeSpan? Duration { get; set; }
}