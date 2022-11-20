using GymTracker.Entities;

namespace GymTracker.Services;

public interface ISessionRepo
{
    // Read
    IEnumerable<Session> GetAllSessions();
    Session? GetSessionById(Guid id);
    bool SessionExists(Guid id);
    
    IEnumerable<Exercise> GetAllExercises();
    Exercise? GetExerciseById(Guid id);
    IEnumerable<string> GetAllExerciseNames();
    bool ExerciseExists(Guid id);

    // Create
    void CreateSession(Session session);
    void AddExerciseToSession(Guid sessionId, Exercise exercise);

    // Update
    void UpdateSession(Session session);
    void UpdateExerciseInSession(Guid sessionId, Exercise exercise);

    // Delete
    void DeleteSession(Guid id);
    void DeleteExerciseFromSession(Guid sessionId, Guid exerciseId);
}