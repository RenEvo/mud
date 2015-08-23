using Akka.Actor;
using Common.Logging;

namespace RenEvo.Mud.Web.Actors
{
    public class TestActor : TypedActor, IHandle<string>
    {
        private readonly ILog _logger;

        public TestActor(ILog logger)
        {
            _logger = logger;
        }

        public void Handle(string message)
        {
            _logger.InfoFormat("Received: {0}", message);

            Sender.Tell("Hello " + message);
        }

        protected override void PostStop()
        {
            base.PostStop();

            _logger.TraceFormat("TestActor Stopped");
        }

        protected override void PreStart()
        {
            base.PreStart();

            _logger.TraceFormat("TestActor Starting");
        }
    }
}
