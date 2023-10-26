using Microsoft.AspNetCore.Mvc;
namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/command/[controller]")]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {

        }

        public ActionResult TestInboundConnection()
        {
            System.Console.WriteLine("Inbound POST # Command Service ");
            return Ok("Inbound test from Platforms controller");
        }
    }
}