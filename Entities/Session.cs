using System.ComponentModel.DataAnnotations;

namespace GymTracker.Entities;

public class Session
{
    [Key] public Guid Id { get; set; }

    [Required] public DateTime StartAt { get; set; }

    [Required] public DateTime EndAt { get; set; }

    [Required] [MaxLength(100)] public string Location { get; set; } = string.Empty;

    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;

    public IList<Exercise> Exercises { get; set; } = new List<Exercise>();
}