using System;
using System.Web.Http;
using Akka.Actor;
using Common.Logging;
using Microsoft.Owin.Hosting;
using Owin;

namespace RenEvo.Mud.Web.Actors
{
    public class OwinActor : ReceiveActor
    {
        private readonly ILog _logger;
        private readonly System.Web.Http.Dependencies.IDependencyResolver _resolver;
        private IDisposable _webInstance;
        private readonly StartOptions _webOptions;

        public OwinActor(ILog logger, System.Web.Http.Dependencies.IDependencyResolver resolver)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));

            _logger = logger;
            _resolver = resolver;

            _webOptions = new StartOptions { Port = 8000 };
            _webOptions.Urls.Add("http://localhost:" + 8000);
        }
        
        protected override void PreStart()
        {
            _webInstance = WebApp.Start(_webOptions, OwinStartup);

            _logger.InfoFormat("OWIN Web Server Started on port {0}", _webOptions.Port);

            Context.Resolve<TestActor>("test");
        }

        private void OwinStartup(IAppBuilder builder)
        {
            var config = new HttpConfiguration
            {
                DependencyResolver = _resolver
            };

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
            builder.UseWebApi(config);
        }

        protected override void PostStop()
        {
            _logger.InfoFormat("OWIN Web Server Stopped on port {0}", _webOptions.Port);

            // TODO: find out why we have to poison pill our children, this is an odd thing
            Context.ActorSelection("/user/owin/test").Tell(PoisonPill.Instance); // the fudge????
            using (_webInstance) {}
        }

    }
}
