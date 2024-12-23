using Confluent.Kafka;
using Shared;
using System.Text.Json;

namespace ProductApi.ProductServices
{
    public class ProductService(IProducer<Null, string> producer) : IProductService
    {

        private List<Product> Products = new List<Product>();
        public async Task AddProduct(Product product)
        {
            Products.Add(product);
            var result = await producer.ProduceAsync("add-product", new Message<Null, string> { Value = JsonSerializer.Serialize(product) });
            if (result.Status != PersistenceStatus.Persisted)
            {
                //Get last product
                var lastProduct = Products.Last();
                //Remove last product
                Products.Remove(lastProduct);

            }
        }

        public async Task DeleteProduct(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                Products.Remove(product);
                await producer.ProduceAsync("delete-product", new Message<Null, string> { Value = id.ToString() });
            }
        }
    }
}
