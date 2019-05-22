using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaUtilities.Actors
{
    public class OrderItem
    {
        public string Article { get; set; }
        public double Quantity { get; set; }
    }

    public class GetOrderItems { }
}
