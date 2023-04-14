using ESourcing.Sourcing.Entities;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Data
{
    public class SourcingContextSeed
    {
        public static void SeedData(IMongoCollection<Auction> mongoCollection)
        {
            if (!mongoCollection.Find(p => true).Any())
            {
                mongoCollection.InsertMany(GetPreConfiguredAuctions());
            }
        }

        private static IEnumerable<Auction> GetPreConfiguredAuctions()
        {
            return new List<Auction>()
            {
                new Auction
                {
                    Name = "A",
                    Description = "B",
                    CreatedAt = DateTime.Now,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now,
                    ProductId = "asdfasfsadfas",
                    IncludedSellers = new List<string>()
                    {
                        "adfsadfsa",
                        "adfsadfsa",
                        "adfsadfsa",
                        "adfsadfsa",
                    },
                    Quantity = 1,
                    Status = (int)Status.Active
                },
                new Auction
                {
                    Name = "C",
                    Description = "D",
                    CreatedAt = DateTime.Now,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now,
                    ProductId = "asdfasfsadfas",
                    IncludedSellers = new List<string>()
                    {
                        "adfsadfsa",
                        "adfsadfsa",
                        "adfsadfsa",
                        "adfsadfsa",
                    },
                    Quantity = 1,
                    Status = (int)Status.Active
                },
                new Auction
                {
                    Name = "E",
                    Description = "F",
                    CreatedAt = DateTime.Now,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now,
                    ProductId = "asdfasfsadfas",
                    IncludedSellers = new List<string>()
                    {
                        "adfsadfsa",
                        "adfsadfsa",
                        "adfsadfsa",
                        "adfsadfsa",
                    },
                    Quantity = 1,
                    Status = (int)Status.Active
                },
            };
        }
    }
}
