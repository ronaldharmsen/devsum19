using Akka.Cluster.Sharding;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaUtilities
{
    public sealed class MessageExtractor : HashCodeMessageExtractor
    {
        public MessageExtractor()
            : base(10)
        { }

        public override string EntityId(object message) => (message as ShardEnvelope)?.EntityId;
        public override object EntityMessage(object message) => (message as ShardEnvelope)?.Message;
    }

    public sealed class ShardEnvelope
    {
        public string EntityId { get; }
        public object Message { get; }

        public ShardEnvelope(string entityId, object message)
        {
            EntityId = entityId;
            Message = message;
        }
    }
}
