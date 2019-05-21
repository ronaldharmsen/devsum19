using System;
using System.Diagnostics;
using System.Net.Http;
using Akka.Actor;
using Akka.Cluster;
using Akka.Routing;
using AkkaUtilities;
using Petabridge.Cmd.Cluster;
using Petabridge.Cmd.Host;
using Petabridge.Cmd.Remote;

namespace AmazingWebshop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var actorSystem = Bootstrap.CreateActorSystem();

            var mainActor = actorSystem.CreateShardedOrderSystem();

            var pbm = PetabridgeCmd.Get(actorSystem);
            pbm.RegisterCommandPalette(ClusterCommands.Instance);
            pbm.RegisterCommandPalette(RemoteCommands.Instance);
            pbm.Start();

            actorSystem.WhenTerminated.Wait();
        }
    }
}
