using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[ApiController]
[Route("api/command/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepository repository;
    private readonly IMapper mapper;

    public PlatformsController(ICommandRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var platformItems = repository.GetAllPlatforms();
        return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));   
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        System.Console.WriteLine("Inbound POST # Command Service ");
        return Ok("Inbound test from Platforms controller");
    }
}
