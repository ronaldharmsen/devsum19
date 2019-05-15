using Akka.Actor;
using Akka.Bootstrap.Docker;

namespace AmazingWebshop.Akka
{
    public static class Bootstrap
    {
        public static ActorSystem CreateActorSystem()
        {
            var config = HoconLoader.FromFile("akka-config.hocon");
            return ActorSystem.Create("amazing", config.BootstrapFromDocker());
        }
    }
}
