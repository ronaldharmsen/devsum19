using Akka;
using Akka.Actor;
using Akka.Event;
using Akka.Persistence;
using AkkaUtilities.Actors;
using System;
using System.Collections.Generic;

namespace AmazingWebshop
{
    public class OrderActor : ReceivePersistentActor
    {
        public override string PersistenceId { get; } = $"{ShardId}/{EntityId}";

        private static string ShardId => Context.Parent.Path.Name;
        private static string EntityId => Context.Self.Path.Name;

        private static readonly TimeSpan InactivityWindow = TimeSpan.FromSeconds(20);
        private readonly ILoggingAdapter _log = Context.GetLogger();
        private readonly List<ItemOrdered> OrderedItems = new List<ItemOrdered>();

        public OrderActor()
        {
            Recover<ItemOrdered>(item => OrderedItems.Add(item));

            CommandAny(Handle);
        }

        public void Handle(object message)
        {
            SetReceiveTimeout(InactivityWindow);

            message
                .Match()
                .With<OrderItem>(HandleMessage)
                .With<ReceiveTimeout>(HandleReceiveTimeout)
                .Default(x => _log.Error("Unhandled message type."));
        }

        private void HandleMessage(OrderItem order)
        {
            _log.Info($"{EntityId} is handling order for {order.Article} x {order.Quantity}");
            Persist(new ItemOrdered(EntityId, order.Article, order.Quantity), item =>
            {
                OrderedItems.Add(item);

                _log.Info($"Item {item.Article} x {item.Quantity} placed in order...");
            });
        }

        private void HandleReceiveTimeout()
        {
            _log.Warning($"{EntityId} is inactive. Shutting down...");

            Context.Stop(Self);
        }

        protected override void PreStart()
        {
            _log.Info($"Waking up order-actor {EntityId} on {ShardId}");
        }
    }

    public class ItemOrdered
    {
        public ItemOrdered(string order, string article, double quantity)
        {
            Order = order;
            Article = article;
            Quantity = quantity;
        }

        public string Order { get; }
        public string Article { get; }
        public double Quantity { get; }
    }
}