using System.Web.Http;
using Akka.Actor;
using Akka.DI.Core;
using Owin;
using RenEvo.Mud.Web.Actors;

namespace RenEvo.Mud.Web
{
    public static class WebStartup
    {
        public static void Configuration(IAppBuilder builder, System.Web.Http.Dependencies.IDependencyResolver resolver)
        {
            InitActors(resolver);

            var config = new HttpConfiguration
            {
                DependencyResolver = resolver
            };

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });

            builder.UseWebApi(config);
        }

        private static void InitActors(System.Web.Http.Dependencies.IDependencyResolver resolver)
        {
            var system = (ActorSystem)resolver.GetService(typeof(ActorSystem));

            system.ActorOf(system.DI().Props<TestActor>(), "Test");
        }
    }
}
