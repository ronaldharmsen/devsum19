using Akka.Actor;
using Akka.Bootstrap.Docker;
using Akka.Cluster.Sharding;
using AmazingWebshop;
using System;
using System.Diagnostics;

namespace AkkaUtilities
{
    public static class Bootstrap
    {
        public static ActorSystem CreateActorSystem()
        {
            if (Debugger.IsAttached) // If we're launching running from visual studio, set CLUSTER_IP
            {
                Environment.SetEnvironmentVariable("CLUSTER_IP", "localhost");
            }

            var config = HoconLoader.FromFile("akka-config.hocon");
            return ActorSystem.Create("amazing", config.BootstrapFromDocker());
        }        

        public static ActorSystem CreateShardedActorSystem()
        {
            if (Debugger.IsAttached) // If we're launching running from visual studio, set CLUSTER_IP
            {
                Environment.SetEnvironmentVariable("CLUSTER_IP", "localhost");
            }

            var config = HoconLoader.FromFile("akka-config.hocon");
            return ActorSystem.Create("amazing", 
                config
                  .WithFallback(ClusterSharding.DefaultConfig())
                  .BootstrapFromDocker());
        }

        public static IActorRef CreateShardedOrderSystem(this ActorSystem system)
        {
            var sharding = ClusterSharding.Get(system);
            var settings = ClusterShardingSettings
                .Create(system)
                .WithRole("demo");

            return sharding.Start(
                typeName: "order",
                entityProps: Props.Create<OrderActor>(),
                settings: settings,
                messageExtractor: new MessageExtractor());
        }

        public static IActorRef CreateOrderShardProxy(this ActorSystem system)
        {
            var sharding = ClusterSharding.Get(system);

            return sharding.StartProxy(
                typeName: "order",
                role: "demo",
                messageExtractor: new MessageExtractor());
        }
    }
}
