using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
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

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {            
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public void Post([FromBody] OrderItemModel item)
        {
            this.orders.OrderItem("1", item.Article, item.Quantity);
        }
    }

    public class OrderItemModel
    {
        public string Article { get; set; }
        public double Quantity { get; set; }
    }
}
