using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaUtilities.Actors;
using Microsoft.AspNetCore.Mvc;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderProcessing orders;

        public OrderController(IOrderProcessing orders)
        {
            this.orders = orders;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<OrderItem>> Get(string id)
        {
            var basket = await this.orders.GetOrderItemsForOrder(id);
            return basket.Items;
        }

        [HttpPost("{orderid}")]
        public void Post(string orderid, [FromBody] OrderItemModel item)
        {
            this.orders.OrderItem(orderid, item.Article, item.Quantity);
        }
    }

    public class OrderItemModel
    {
        public string Article { get; set; }
        public double Quantity { get; set; }
    }
}
