using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo repo;
        private readonly IMapper mapper;
        private readonly ICommandDataClient commandDataClient;

        public PlatformsController(IPlatformRepo repo, IMapper mapper, ICommandDataClient commandDataClient)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platforms = repo.GetAllPlatforms();
            return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet]
        [Route("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = repo.GetPlatformById(id);
            if (platform != null)
                return Ok(mapper.Map<PlatformReadDto>(platform));
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePlatform(PlatformCreateDto platform)
        {
            var model = mapper.Map<Platform>(platform);
            repo.CreatePlatform(model);
            repo.SaveChanges();
            var platformReadDto = mapper.Map<PlatformReadDto>(model);
            try
            {
                await commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Could not send synchronously: {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlatformById), new { model.Id }, platformReadDto);
        }


    }
}