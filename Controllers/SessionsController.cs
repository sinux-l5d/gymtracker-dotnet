using AutoMapper;
using GymTracker.Dto;
using GymTracker.Entities;
using GymTracker.Services;

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
    public ActionResult<List<SessionDto>> Get()
    {
        var sessions = _sessionRepo.GetAllSessions();
        //return Ok(_mapper.Map<IEnumerable<SessionDto>>(sessions));
        return Ok(_mapper.Map<List<SessionDto>>(sessions));
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

    // PUT: api/Session/5
    // [HttpPut("{id}")]
    // public void Put(int id, [FromBody] string value)
    // {
    // }

    // DELETE: api/Session/5
    // [HttpDelete("{id}")]
    // public void Delete(int id)
    // {
    // }
}