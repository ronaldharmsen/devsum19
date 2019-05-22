using AmazingWebshop;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    public interface IOrderProcessing
    {
        Task<ShoppingBasket> GetOrderItemsForOrder(string orderId);
        void OrderItem(string id, string article, double quantity);
    }
}