using ESourcing.Products.Entities;
using MongoDB.Driver;

namespace ESourcing.Products.Data
{
    public class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if(!existProduct)
            {
                productCollection.InsertManyAsync(GetConfigureProducts());
            }
        }

        private static IEnumerable<Product> GetConfigureProducts()
        {
            return new List<Product>()
            {
                new Product
                {
                    Name = "deneme1",
                    Summary = "deneme1",
                    Description = "deneme1",
                    ImageFile = "deneme1",
                    Price = 1,
                    Category = "deneme1"
                },
                new Product
                {
                    Name = "deneme2",
                    Summary = "deneme2",
                    Description = "deneme2",
                    ImageFile = "deneme2",
                    Price = 2,
                    Category = "deneme2"
                },
                new Product
                {
                    Name = "deneme3",
                    Summary = "deneme3",
                    Description = "deneme3",
                    ImageFile = "deneme3",
                    Price = 3,
                    Category = "deneme3"
                }
            };
        }
    }
}
