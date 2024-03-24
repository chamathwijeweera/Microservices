using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[ApiController]
[Route("api/command/platforms/{platformId}/[controller]")]
public class CommandsController : Controller
{
    private readonly ICommandRepository repository;
    private readonly IMapper mapper;

    public CommandsController(ICommandRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        if (!repository.PlatformExits(platformId))
            return NotFound();

        var commands = repository.GetCommandsForPlatform(platformId);
        return Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId}", Name ="CommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        if (!repository.PlatformExits(platformId))
            return NotFound();
        var command = repository.GetCommand(platformId, commandId);
        if(command==null)
            return NotFound();  
        return Ok(mapper.Map<CommandReadDto>(command));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
        if(!repository.PlatformExits(platformId))
            return NotFound();
        var command = mapper.Map<Command>(commandDto);
        repository.CreateCommand(platformId , command);
        repository.SaveChanges();
        var commandReadDto = mapper.Map<CommandReadDto>(command);
        return CreatedAtRoute("CommandForPlatform",
            new { platformId, commandId = commandReadDto.Id},commandReadDto);
    }
}

