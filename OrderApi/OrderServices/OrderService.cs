using Confluent.Kafka;
using Shared;
using System.Text.Json;
namespace OrderApi.OrderServices
{
    public class OrderService(IConsumer<Null, string> consumer) : IOrderService
    {
        private const string AddProductTopic = "add-product-topic";
        private const string DeleteProductTopic = "delete-product-topic";
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Order> Orders { get; set; } = new List<Order>();

        public async Task StartConsumingService()
        {
            await Task.Delay(1000);
            consumer.Subscribe(new[] { AddProductTopic, DeleteProductTopic });
            while (true)
            {
                var response = consumer.Consume();
                if (!string.IsNullOrWhiteSpace(response.Message.Value))
                {
                    if (response.Topic == AddProductTopic)
                    {
                        var product = JsonSerializer.Deserialize<Product>(response.Message.Value);
                        if (product != null)
                        {
                            Products.Add(product);
                        }
                    }
                    else if (response.Topic == DeleteProductTopic)
                    {
                        var productId = int.Parse(response.Message.Value);
                        var product = Products.FirstOrDefault(p => p.Id == productId);
                        if (product != null)
                        {
                            Products.Remove(product);
                        }
                    }
                    ConsoleProduct();
                }
            }
        }
        private void ConsoleProduct()
        {
            Console.Clear();
            foreach (var item in Products)
            {
                Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Price: {item.Price}");
            }
        }
        public void AddOrder(Order order) => Orders.Add(order);

        public List<OrderSummary> GetOrderSummaries()
        {
            var orderSummaries = new List<OrderSummary>();
            foreach (var item in Orders)
            {
                var product = Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    orderSummaries.Add(new OrderSummary()
                    {
                        OderId = item.Id,
                        OrderedQuantity = item.Quantity,
                        ProductId = item.ProductId,
                        ProductPrice = product.Price,
                        ProductName = product.Name
                    });
                }
            }
            return orderSummaries;
        }

        public List<Product> GetProducts() => Products;
    }
}

