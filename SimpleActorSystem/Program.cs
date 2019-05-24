using Akka.Actor;
using Akka.Configuration;
using Akka.Event;
using System;

namespace SimpleActorSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("simple");

            var actor = system.ActorOf<SimpleActor>("actorA");
            actor.Tell(10);
            actor.Tell(20);

            system.WhenTerminated.Wait();
        }
    }

    public class SimpleActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();
        private int state = 0;

        public SimpleActor()
        {
            Receive<int>(number =>
            {
                log.Info($"Adding {number}");
                state += number;

                log.Info($"Total is now {state}");
            });

            Receive<string>(text =>
            {
                Sender.Tell("echo:" + text);
            });
        }
    }

    #region extra
    //actor.Ask("hello").ContinueWith(t => {
    //    Console.ForegroundColor = ConsoleColor.Magenta;
    //    Console.WriteLine(t.Result);
    //    Console.ResetColor();
    //});

    //var sub = Context.ActorOf<OtherActor>();
    //sub.Tell(number);


    public class OtherActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();

        public OtherActor()
        {
            Receive<int>(nr =>
            {
                log.Info($"Hello from actor {Self.Path}. Retrieving nr {nr} from {Context.Parent}");
            });
        }
    }
    #endregion
}
