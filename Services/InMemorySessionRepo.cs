using GymTracker.Entities;

namespace GymTracker.Services;

// Development purpose, temporary
public class InMemorySessionRepo : ISessionRepo
{
    private readonly IList<Session> _sessions;

    public InMemorySessionRepo(IList<Session>? sessions = null)
    {
        // If sessions is not null, just set. Otherwise, create a new list with items
        _sessions = sessions ?? new List<Session>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Upper Body",
                StartAt = DateTime.Now,
                EndAt = DateTime.Now.AddHours(1)
            }
        };
    }

    // Read

    public IEnumerable<Session> GetAllSessions()
    {
        return _sessions;
    }

    public Session? GetSessionById(Guid id)
    {
        return _sessions.FirstOrDefault(s => s.Id == id);
    }

    public bool SessionExists(Guid id)
    {
        return _sessions.Any(s => s.Id == id);
    }


    public IEnumerable<Exercise> GetAllExercises()
    {
        return _sessions.SelectMany(s => s.Exercises);
    }

    public Exercise? GetExerciseById(Guid id)
    {
        return GetAllExercises().FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<string> GetAllExerciseNames()
    {
        return GetAllExercises().Select(e => e.Name);
    }

    public bool ExerciseExists(Guid id)
    {
        return GetAllExercises().Any(e => e.Id == id);
    }


    // Create

    public void CreateSession(Session session)
    {
        session.Id = Guid.NewGuid();
        _sessions.Add(session);
    }

    public void AddExerciseToSession(Guid sessionId, Exercise exercise)
    {
        if (sessionId == Guid.Empty) throw new ArgumentNullException(nameof(sessionId));

        if (exercise == null) throw new ArgumentNullException(nameof(exercise));

        var session = GetSessionById(sessionId);
        if (session == null) throw new ArgumentException(nameof(sessionId));

        exercise.Id = Guid.NewGuid();
        session.Exercises.Add(exercise);
    }

    // Update

    /*
     * UpdateSession update the corresponding session in the list of sessions.
     * The session id should be in the argument session.
     */
    public void UpdateSession(Session session)
    {
        var id = session.Id;
        var existingSession = GetSessionById(id);

        if (existingSession == null) throw new ArgumentException(nameof(session.Id));

        // Update every field except Id
        existingSession.Name = session.Name;
        existingSession.StartAt = session.StartAt;
        existingSession.EndAt = session.EndAt;
        existingSession.Location = session.Location;
    }

    public void UpdateExerciseInSession(Guid sessionId, Exercise exercise)
    {
        var existingSession = GetSessionById(sessionId);
        if (existingSession == null) throw new ArgumentException(nameof(sessionId));

        var existingExercise = existingSession.Exercises.FirstOrDefault(e => e.Id == exercise.Id);
        if (existingExercise == null) throw new ArgumentException(nameof(exercise.Id));

        // Update every field except Id
        existingExercise.Name = exercise.Name;
        existingExercise.Description = exercise.Description;
        existingExercise.Repetitions = exercise.Repetitions;
    }

    // Delete

    public void DeleteSession(Guid id)
    {
        var session = GetSessionById(id);
        if (session == null) throw new ArgumentException(nameof(id));

        _sessions.Remove(session);
    }

    public void DeleteExerciseFromSession(Guid sessionId, Guid exerciseId)
    {
        var session = GetSessionById(sessionId);
        if (session == null) throw new ArgumentException(nameof(sessionId));

        var exercise = session.Exercises.FirstOrDefault(e => e.Id == exerciseId);
        if (exercise == null) throw new ArgumentException(nameof(exerciseId));

        session.Exercises.Remove(exercise);
    }
}