namespace GymTracker.Exceptions;

public class ExerciseNotInSessionException : Exception
{
    public Guid ExerciseId { get; }
    public Guid SessionId { get; }

    public ExerciseNotInSessionException(Guid exerciseId, Guid sessionId) : base(
        $"Exercise '{exerciseId}' is not in session '{sessionId}'")
    {
        ExerciseId = exerciseId;
        SessionId = sessionId;
    }
}