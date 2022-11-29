using GymTracker.Entities;
using GymTracker.Exceptions;

namespace GymTracker.Services;

public interface ISessionRepo
{
    // Read
    IEnumerable<Session> GetAllSessions();
    Session? GetSessionById(Guid id);
    bool SessionExists(Guid id);

    IEnumerable<Exercise> GetAllExercises();
    Exercise? GetExerciseById(Guid id);
    IEnumerable<Exercise> GetExercisesBySessionId(Guid id);
    IEnumerable<string> GetAllExerciseNames();
    bool ExerciseExists(Guid id);

    // Create
    Session CreateSession(Session session);
    void AddExerciseToSession(Guid sessionId, Exercise exercise);

    // Update

    /// <summary>
    /// Update a session based on session.Id
    /// </summary>
    /// <param name="session"></param>
    /// <exception cref="SessionNotFoundException"></exception>
    void UpdateSession(Session session);

    /// <summary>
    /// Update an exercise belonging to a session
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="exercise"></param>
    /// <exception cref="SessionNotFoundException"></exception>
    /// <exception cref="ExerciseNotFoundException"></exception>
    /// <exception cref="ExerciseNotInSessionException"></exception>
    void UpdateExerciseInSession(Guid sessionId, Exercise exercise);

    // Delete

    /// <summary>
    /// Delete a session and all exercises belonging to it
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="SessionNotFoundException"></exception>
    void DeleteSession(Guid id);

    void DeleteExerciseFromSession(Guid sessionId, Guid exerciseId);
}