using System;
using System.Diagnostics;
using Akka.Actor;
using Akka.Cluster;
using Akka.Routing;

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

            var clusterMember = Cluster.Get(actorSystem);

            clusterMember.RegisterOnMemberUp(() =>
            {
                if (Environment.GetEnvironmentVariable("CLUSTER_IP")?.ToLower() == "seed")
                    actorSystem.Scheduler.Advanced.ScheduleRepeatedly(TimeSpan.FromSeconds(3), TimeSpan.FromMilliseconds(100), () =>
                    {
                        router.Tell(count++, echo);
                    });
            });

            actorSystem.WhenTerminated.Wait();
        }
    }
}
