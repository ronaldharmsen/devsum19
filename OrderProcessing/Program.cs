using System;
using System.Diagnostics;
using System.Net.Http;
using Akka.Actor;
using Akka.Cluster;
using Akka.Routing;
using Petabridge.Cmd.Cluster;
using Petabridge.Cmd.Host;
using Petabridge.Cmd.Remote;

namespace AmazingWebshop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (Debugger.IsAttached) // If we're launching running from visual studio, set CLUSTER_IP
            {
                Environment.SetEnvironmentVariable("CLUSTER_IP", "localhost");
            }

            var actorSystem = Akka.Bootstrap.CreateActorSystem();
            var echo = actorSystem.ActorOf(Props.Create(() => new EchoActor()), "echo");
            var router = actorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "router");

            int count = 0;

            var pbm = PetabridgeCmd.Get(actorSystem);
            pbm.RegisterCommandPalette(ClusterCommands.Instance);
            pbm.RegisterCommandPalette(RemoteCommands.Instance);
            pbm.Start();

            var clusterMember = Cluster.Get(actorSystem);

            clusterMember.RegisterOnMemberUp(() =>
            {
                if (Environment.GetEnvironmentVariable("CLUSTER_IP")?.ToLower() == "seed")
                    actorSystem.Scheduler.Advanced.ScheduleRepeatedly(TimeSpan.FromSeconds(3), TimeSpan.FromMilliseconds(1), () =>
                    {
                        router.Tell(count++, echo);
                    });
            });

            actorSystem.WhenTerminated.Wait();
        }
    }
}
