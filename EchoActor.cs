using Akka.Actor;
using Akka.Event;
using System;

namespace AmazingWebshop
{
    public sealed class EchoActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        public EchoActor()
        {
            ReceiveAny(_ =>
            {
                _log.Info("Received {0} from {1}", _, Sender);
            });
        }
    }
}
