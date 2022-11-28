using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymTracker.Entities;

public class Exercise
{
    [Key] public Guid Id { get; set; }

    public Guid SessionId { get; set; }

    [Required] [MaxLength(30)] public string Name { get; set; } = string.Empty;

    [MaxLength(500)] public string Description { get; set; } = string.Empty;

    public int Repetitions { get; set; }
}