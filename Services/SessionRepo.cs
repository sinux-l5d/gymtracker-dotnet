using GymTracker.Data;
using GymTracker.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace GymTracker.Services;

public class SessionRepo : ISessionRepo
{
    private readonly DataContext _context;

    public SessionRepo(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<Session> GetAllSessions()
    {
        // Sessions with exercises. The Exercice contains the sessionId it is attached to
        return _context.Sessions
            .Include(s => s.Exercises)
            .ToList();
    }

    public Session? GetSessionById(Guid id)
    {
        return _context.Sessions.FirstOrDefault(s => s.Id == id);
    }

    public bool SessionExists(Guid id)
    {
        return _context.Sessions.Any(s => s.Id == id);
    }

    public IEnumerable<Exercise> GetAllExercises()
    {
        return _context.Exercises.ToList();
    }

    public Exercise? GetExerciseById(Guid id)
    {
        return _context.Exercises.FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Exercise> GetExercisesBySessionId(Guid id)
    {
        return _context.Exercises.Where(e => e.SessionId == id).ToList();
    }

    public IEnumerable<string> GetAllExerciseNames()
    {
        return _context.Exercises.Select(e => e.Name).ToList();
    }

    public bool ExerciseExists(Guid id)
    {
        return _context.Exercises.Any(e => e.Id == id);
    }

    public void CreateSession(Session session)
    {
        session.Id = Guid.NewGuid();
        _context.Sessions.Add(session);
        _context.SaveChanges();
    }

    public void AddExerciseToSession(Guid sessionId, Exercise exercise)
    {
        exercise.Id = Guid.NewGuid();
        exercise.SessionId = sessionId;
        _context.Exercises.Add(exercise);
        //_context.Sessions.FirstOrDefault(s => s.Id == sessionId)?.Exercises.Add(exercise);
        _context.SaveChanges();
    }

    // The parameters doesn't have every data
    // We should update only the data that is provided
    public void UpdateSession(Session session)
    {
        var sessionDb = _context.Sessions.FirstOrDefault(s => s.Id == session.Id);
        if (sessionDb == null) return;

        sessionDb.Name = session.Name;
        sessionDb.Location = session.Location;
        sessionDb.StartAt = session.StartAt;
        sessionDb.EndAt = session.EndAt;

        _context.SaveChanges();
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