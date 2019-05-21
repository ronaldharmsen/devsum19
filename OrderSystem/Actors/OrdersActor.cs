using Akka.Actor;
using Akka.Event;
using AkkaUtilities.Actors;
using System;

namespace AmazingWebshop
{
    public sealed class OrdersActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        
        public OrdersActor()
        {
            Receive<OrderItem>(order =>
            {
                var processor = Context.ActorOf<OrderActor>("1");
                processor.Tell(order);
            });

            ReceiveAny(_ =>
            {
                _log.Info("Received {0} from {1}", _, Sender);
            });
        }
    }
}
