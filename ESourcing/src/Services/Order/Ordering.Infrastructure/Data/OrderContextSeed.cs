using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext)
        {
            if(!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "test1@test.com",
                    UnitPrice = 1,
                    TotalPrice = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Order
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "test2@test.com",
                    UnitPrice = 1,
                    TotalPrice = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Order
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "test3@test.com",
                    UnitPrice = 1,
                    TotalPrice = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Order
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "test4@test.com",
                    UnitPrice = 1,
                    TotalPrice = 2,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}
