using System.Threading.Tasks;
using System.Web.Http;
using Akka.Actor;
using Common.Logging;

namespace RenEvo.Mud.Web.Controllers
{
    [RoutePrefix("api/akka")]
    public class TestController : ApiController
    {
        private readonly ILog _logger;
        private readonly IActorRefFactory _system;

        public TestController(ILog logger, IActorRefFactory system)
        {
            _logger = logger;
            _system = system;
        }

        [Route(""), HttpGet]
        public async Task<object> GetAsync(string name = null)
        {
            var result = await _system.ActorSelection("/user/Test").Ask<string>(name ?? "Steve");

            _logger.WarnFormat("TestController /api/akka returned: {0}", result);
            return new { result = result };
        }
    }
}
