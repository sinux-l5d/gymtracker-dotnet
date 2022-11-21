using GymTracker.Data;
using GymTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Services;

public class SessionRepo : ISessionRepo
{
    private readonly DataContext _context;
    private readonly ILogger<SessionRepo> _logger;

    public SessionRepo(DataContext context, ILogger<SessionRepo> logger)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<Session> GetAllSessions()
    {
        // Log the call
        _logger.LogInformation("Getting all sessions");

        return _context.Sessions.ToList();
    }

    public Session? GetSessionById(Guid id)
    {
        return _context.Sessions.FirstOrDefault(s => s.Id == id);
    }

    public bool SessionExists(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Exercise> GetAllExercises()
    {
        throw new NotImplementedException();
    }

    public Exercise? GetExerciseById(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetAllExerciseNames()
    {
        throw new NotImplementedException();
    }

    public bool ExerciseExists(Guid id)
    {
        throw new NotImplementedException();
    }

    public void CreateSession(Session session)
    {
        session.Id = Guid.NewGuid();
        _context.Sessions.Add(session);
        _context.SaveChanges();
    }

    public void AddExerciseToSession(Guid sessionId, Exercise exercise)
    {
        throw new NotImplementedException();
    }

    public void UpdateSession(Session session)
    {
        throw new NotImplementedException();
    }

    public void UpdateExerciseInSession(Guid sessionId, Exercise exercise)
    {
        throw new NotImplementedException();
    }

    public void DeleteSession(Guid id)
    {
        throw new NotImplementedException();
    }

    public void DeleteExerciseFromSession(Guid sessionId, Guid exerciseId)
    {
        throw new NotImplementedException();
    }
}