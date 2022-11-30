using GymTracker.Data;
using GymTracker.Entities;
using GymTracker.Exceptions;
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

    public Session CreateSession(Session session)
    {
        session.Id = Guid.NewGuid();
        var sessionDb = _context.Sessions.Add(session);
        _context.SaveChanges();
        return sessionDb.Entity;
    }

    public Exercise AddExerciseToSession(Guid sessionId, Exercise exercise)
    {
        exercise.Id = Guid.NewGuid();
        exercise.SessionId = sessionId;
        _context.Exercises.Add(exercise);
        //_context.Sessions.FirstOrDefault(s => s.Id == sessionId)?.Exercises.Add(exercise);
        _context.SaveChanges();
        return exercise;
    }

    public void UpdateSession(Session session)
    {
        var sessionDb = _context.Sessions.FirstOrDefault(s => s.Id == session.Id);
        // Session not found
        if (sessionDb == null) throw new SessionNotFoundException(session.Id);

        sessionDb.Name = session.Name;
        sessionDb.Location = session.Location;
        sessionDb.StartAt = session.StartAt;
        sessionDb.EndAt = session.EndAt;

        _context.SaveChanges();
    }

    public void UpdateExerciseInSession(Guid sessionId, Exercise exercise)
    {
        var sessionDb = _context.Sessions.FirstOrDefault(s => s.Id == sessionId);
        if (sessionDb == null) throw new SessionNotFoundException(sessionId);

        var exerciseDb = _context.Exercises.FirstOrDefault(e => e.Id == exercise.Id);
        if (exerciseDb == null) throw new ExerciseNotFoundException(exercise.Id);

        // check exercise is in session
        if (exerciseDb.SessionId != sessionId) throw new ExerciseNotInSessionException(exercise.Id, sessionId);

        exerciseDb.Name = exercise.Name;
        exerciseDb.Description = exercise.Description;
        exerciseDb.Repetitions = exercise.Repetitions;

        _context.SaveChanges();
    }

    public void DeleteSession(Guid id)
    {
        var sessionDb = _context.Sessions.FirstOrDefault(s => s.Id == id);
        if (sessionDb == null) throw new SessionNotFoundException(id);

        _context.Sessions.Remove(sessionDb);

        var exercises = _context.Exercises.Where(e => e.SessionId == id);
        _context.Exercises.RemoveRange(exercises);

        _context.SaveChanges();
    }

    public void DeleteExerciseFromSession(Guid sessionId, Guid exerciseId)
    {
        var sessionDb = _context.Sessions.FirstOrDefault(s => s.Id == sessionId);
        if (sessionDb == null) throw new SessionNotFoundException(sessionId);

        var exerciseDb = _context.Exercises.FirstOrDefault(e => e.Id == exerciseId);
        if (exerciseDb == null) throw new ExerciseNotFoundException(exerciseId);

        // check exercise is in session
        if (exerciseDb.SessionId != sessionId) throw new ExerciseNotInSessionException(exerciseId, sessionId);

        _context.Exercises.Remove(exerciseDb);
        _context.SaveChanges();
    }
}