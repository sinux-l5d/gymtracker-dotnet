using AutoMapper;
using GymTracker.Dto;
using GymTracker.Entities;
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
    public IActionResult Post(Session session)
    {
        try
        {
            _sessionRepo.CreateSession(session);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost("{id}/exercise")]
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

        return Ok();
    }

    [HttpGet("{id}/exercise")]
    public ActionResult<IEnumerable<ExerciseDto>> GetExercises(Guid id)
    {
        var exercises = _sessionRepo.GetExercisesBySessionId(id);
        return Ok(_mapper.Map<IEnumerable<ExerciseDto>>(exercises));
    }

    //PATCH: api/Session/5
    [HttpPatch("{id}")]
    public void Patch(Guid id, UpdateSessionDto updateSessionDto)
    {
        var sessionDb = _sessionRepo.GetSessionById(id);
        if (sessionDb == null) return;

        // Automapper need at least StartAt and Duration to map to a Session
        updateSessionDto.Name ??= sessionDb.Name;
        updateSessionDto.Location ??= sessionDb.Location;
        updateSessionDto.StartAt ??= sessionDb.StartAt;
        updateSessionDto.Duration ??= sessionDb.EndAt - sessionDb.StartAt;

        Console.WriteLine("===================================");
        Console.WriteLine("sessionDb: " + sessionDb.ToJson());
        Console.WriteLine("===================================");
        var session = _mapper.Map<Session>(updateSessionDto);
        session.Id = id;
        _sessionRepo.UpdateSession(session);
    }


    // DELETE: api/Session/5
    // [HttpDelete("{id}")]
    // public void Delete(int id)
    // {
    // }
}