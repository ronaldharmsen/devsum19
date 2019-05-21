namespace OrderApi.Controllers
{
    public interface IOrderProcessing
    {
        void OrderItem(string id, string article, double quantity);
    }
}