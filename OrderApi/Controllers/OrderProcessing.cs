using Akka.Actor;
using AkkaUtilities;
using AkkaUtilities.Actors;
using AmazingWebshop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    public class OrderProcessing : IOrderProcessing
    {
        private readonly IActorRef router;

        public OrderProcessing(IActorRef router)
        {
            this.router = router;
        }

        public async Task<ShoppingBasket> GetOrderItemsForOrder(string orderId)
        {
            var envelope = new ShardEnvelope(orderId, new GetOrderItems());
            return await router.Ask<ShoppingBasket>(envelope);
        }

        public void OrderItem(string id, string article, double quantity)
        {
            var message = new OrderItem { Article = article, Quantity = quantity };
            var envelope = new ShardEnvelope(id, message);
            router.Tell(envelope);
        }
    }
}
