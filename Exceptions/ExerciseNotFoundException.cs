namespace GymTracker.Exceptions;

public class ExerciseNotFoundException : NotFoundException
{
    public ExerciseNotFoundException(Guid id) : base("Exercise", id)
    {
    }
}