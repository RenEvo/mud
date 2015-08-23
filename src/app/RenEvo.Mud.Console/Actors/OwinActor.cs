using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Common.Logging;

namespace RenEvo.Mud.Actors
{
    public class OwinActor : Akka.Actor.ReceiveActor
    {
        private readonly ILog _logger;

        public OwinActor(ILog logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;


            Receive<string>((Action<string>)OnReceive);
        }

        private void OnReceive(string value)
        {
            _logger.DebugFormat("received me some stuff: {0}", value);
        }
    }
}
