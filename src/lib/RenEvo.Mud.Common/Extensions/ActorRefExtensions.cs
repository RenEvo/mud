using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;

namespace RenEvo.Mud
{
    public static class ActorRefExtensions
    {
        public static IActorRef Resolve<TActor>(this ActorSystem system, string name) where TActor : ActorBase
        {
            return system.ActorOf(system.DI().Props<TActor>(), name);
        }

        public static IActorRef Resolve<TActor>(this IUntypedActorContext context, string name) where TActor : ActorBase
        {
            return context.ActorOf(context.DI().Props<TActor>(), name);
        }
    }
}
