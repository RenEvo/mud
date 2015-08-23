using System;
using Akka.Actor;
using Common.Logging;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using Owin;
using RenEvo.Mud.Unity;

namespace RenEvo.Mud
{
    public class Bootstrap : IDisposable
    {
        private readonly IUnityContainer _container;
        private readonly ActorSystem _system;
        private readonly IDisposable _web;

        public Bootstrap(string path = "./config", string systemName = "MyAkka", int port = 8000)
        {
            // TODO: read config path
            _container = new UnityContainer();
            _container.RegisterInstance(LogManager.Adapter);
            _container.RegisterInstance(LogManager.GetLogger("Default"));

            _system = InitializeSystem(systemName);
            _web = InitializeWeb(port);
        }
       
        public ActorSystem InitializeSystem(string systemName)
        {
            var system = ActorSystem.Create(systemName);

            var resolver = new Akka.DI.Unity.UnityDependencyResolver(_container, system);
            _container.RegisterInstance<IActorRefFactory>(system);
            _container.RegisterInstance(system);

            return system;
        }

        private IDisposable InitializeWeb(int port)
        {
            if (_system == null)
            {
                throw new InvalidOperationException("You must first initialize the System");
            }

            var options = new StartOptions { Port = port };
            options.Urls.Add("http://localhost:" + port);

            return WebApp.Start(options, OwinStartup);
        }

        public bool Shutdown(TimeSpan timeout)
        {
            _system.Shutdown();
            return _system.AwaitTermination(timeout);
        }

        private void OwinStartup(IAppBuilder builder)
        {
            Web.WebStartup.Configuration(builder, new UnityDependencyResolver(_container));
        }

        // TODO: didn't implement IDisposable right, make it happen
        public void Dispose()
        {
            _web.Dispose();
        }
    }
}
