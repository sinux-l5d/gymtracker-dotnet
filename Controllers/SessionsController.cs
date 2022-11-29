using AutoMapper;
using GymTracker.Dto;
using GymTracker.Entities;
using GymTracker.Exceptions;
using GymTracker.Services;
using NuGet.Protocol;

namespace GymTracker.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SessionsController : ControllerBase
{
    private readonly ISessionRepo _sessionRepo;
    private readonly IMapper _mapper;

    public SessionsController(ISessionRepo sessionRepo, IMapper mapper)
    {
        _sessionRepo = sessionRepo;
        _mapper = mapper;
    }

    // GET: api/session
    [HttpGet]
    public ActionResult<IEnumerable<SessionDto>> Get()
    {
        var sessions = _sessionRepo.GetAllSessions();
        return Ok(_mapper.Map<IEnumerable<SessionDto>>(sessions));
    }

    // GET: api/session/5
    [HttpGet("{id}")]
    public ActionResult<SessionDto> Get(Guid id)
    {
        var session = _sessionRepo.GetSessionById(id);
        if (session == null) return NotFound();
        return Ok(_mapper.Map<SessionDto>(session));
    }

    // POST: api/session
    // @todo return url to new resource
    [HttpPost]
    public ActionResult<SessionDto> Post(AddSessionDto addSessionDto)
    {
        Session session;
        try
        {
            session = _mapper.Map<Session>(addSessionDto);
        }
        catch (AutoMapperMappingException e)
        {
            //return BadRequest(new { error = "Must specify at least name, location and endAt or duration" });
            return BadRequest(e.Message);
        }

        Session sessionDb;
        try
        {
            sessionDb = _sessionRepo.CreateSession(session);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest(ex.Message);
        }

        return CreatedAtAction(nameof(Get), new { id = session.Id }, _mapper.Map<SessionDto>(sessionDb));
    }

    [HttpPost("{id}/exercises")]
    public IActionResult AddExercise(Guid id, AddExerciseDto exerciseDto)
    {
        var exercise = _mapper.Map<Exercise>(exerciseDto);
        try
        {
            _sessionRepo.AddExerciseToSession(id, exercise);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest();
        }

        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpGet("{id}/exercises")]
    public ActionResult<IEnumerable<ExerciseDto>> GetExercises(Guid id)
    {
        var exercises = _sessionRepo.GetExercisesBySessionId(id);
        Console.WriteLine(exercises.ToJson());
        return Ok(_mapper.Map<IEnumerable<ExerciseDto>>(exercises));
    }

    [HttpGet("{id}/exercises/{exerciseId}")]
    public ActionResult<ExerciseDto> GetExercise(Guid id, Guid exerciseId)
    {
        var exercise = _sessionRepo.GetExerciseById(exerciseId); // @todo not checking session
        if (exercise == null) return NotFound();
        Console.WriteLine(exercise.ToJson());
        return Ok(_mapper.Map<ExerciseDto>(exercise));
    }

    [HttpPatch("{id}/exercises/{exerciseId}")]
    public IActionResult UpdateExercise(Guid id, Guid exerciseId, UpdateExerciseDto updateExerciseDto)
    {
        var exerciseDb = _sessionRepo.GetExerciseById(exerciseId);
        if (exerciseDb == null) return NotFound();

        updateExerciseDto.Name ??= exerciseDb.Name;
        updateExerciseDto.Repetitions ??= exerciseDb.Repetitions;
        updateExerciseDto.Description ??= exerciseDb.Description;

        var exercise = _mapper.Map<Exercise>(updateExerciseDto);
        exercise.Id = exerciseId;
        exercise.SessionId = id;
        try
        {
            _sessionRepo.UpdateExerciseInSession(id, exercise);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }


    //PATCH: api/Session/5
    [HttpPatch("{id}")]
    public IActionResult Patch(Guid id, UpdateSessionDto updateSessionDto)
    {
        var sessionDb = _sessionRepo.GetSessionById(id);
        if (sessionDb == null) return NotFound();

        // Automapper need at least StartAt and Duration to map to a Session
        updateSessionDto.Name ??= sessionDb.Name;
        updateSessionDto.Location ??= sessionDb.Location;
        updateSessionDto.StartAt ??= sessionDb.StartAt;
        updateSessionDto.Duration ??= sessionDb.EndAt - sessionDb.StartAt;

        var session = _mapper.Map<Session>(updateSessionDto);
        session.Id = id;
        try
        {
            _sessionRepo.UpdateSession(session);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }


    //DELETE: api/session/5
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            _sessionRepo.DeleteSession(id);
        }
        catch (SessionNotFoundException e)
        {
            return BadRequest(new { error = e.Message });
        }

        return Ok();
    }
}