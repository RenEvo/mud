using System;
using Akka.Actor;
using Akka.DI.Core;
using Common.Logging;
using Microsoft.Practices.Unity;
using RenEvo.Mud.Web.Actors;

namespace RenEvo.Mud
{
    // TODO: implement good disposal pattern
    public class Bootstrap : IDisposable
    {
        private readonly IUnityContainer _container;
        private readonly ActorSystem _system;

        public Bootstrap(string path = "./config", string systemName = "mud")
        {
            // TODO: read config path
            _container = new UnityContainer();
            _container.RegisterInstance(LogManager.Adapter);
            _container.RegisterInstance(LogManager.GetLogger("Default"));
            _system = InitializeSystem(systemName);
        }
       
        public ActorSystem InitializeSystem(string systemName)
        {
            var system = ActorSystem.Create(systemName);

            var resolver = new Akka.DI.Unity.UnityDependencyResolver(_container, system);
            _container.RegisterInstance<IActorRefFactory>(system);
            _container.RegisterInstance(system);
            _container.RegisterInstance<System.Web.Http.Dependencies.IDependencyResolver>(new Unity.UnityDependencyResolver(_container));

            // TODO: need to switch this to support clustering properly
            system.Resolve<OwinActor>("owin");

            return system;
        }

        void IDisposable.Dispose()
        {
            _system.Shutdown();
            _system.AwaitTermination();
            _system.Dispose();
        }
    }
}
