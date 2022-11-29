namespace GymTracker.Exceptions;

[Serializable]
public class SessionNotFoundException : NotFoundException
{
    public Guid SessionId { get; }

    public SessionNotFoundException(Guid id) : base("Session", id)
    {
        SessionId = id;
    }
}