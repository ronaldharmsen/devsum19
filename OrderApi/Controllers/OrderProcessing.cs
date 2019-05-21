using Akka.Actor;
using AkkaUtilities;
using AkkaUtilities.Actors;

namespace OrderApi.Controllers
{
    public class OrderProcessing : IOrderProcessing
    {
        private readonly IActorRef router;

        public OrderProcessing(IActorRef router)
        {
            this.router = router;
        }

        public void OrderItem(string id, string article, double quantity)
        {
            var message = new OrderItem { Article = article, Quantity = quantity };
            var envelope = new ShardEnvelope(id, message);
            router.Tell(envelope);
        }
    }
}
