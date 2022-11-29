namespace GymTracker.Exceptions;

[Serializable]
public class NotFoundException : Exception
{
    public NotFoundException(string objectName, Guid id) : base($"{objectName} with id '{id}' not found")
    {
    }
}