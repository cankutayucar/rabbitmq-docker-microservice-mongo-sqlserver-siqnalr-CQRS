using ESourcing.Sourcing.Data.Interfaces;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ISourcingContext _context;
        public BidRepository(ISourcingContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetBidsByAuctionId(string id)
        {
            var filter = Builders<Bid>.Filter.Eq(a => a.AuctionId, id);
            var bids = await _context.Bids.Find(filter).ToListAsync();
            bids = bids.OrderByDescending(a => a.CreatedAt).GroupBy(a => a.SellerUserName).Select(a => new Bid
            {
                AuctionId = a.FirstOrDefault().AuctionId,
                CreatedAt = a.FirstOrDefault().CreatedAt,
                Id = a.FirstOrDefault().Id,
                Price = a.FirstOrDefault().Price,
                ProductId = a.FirstOrDefault().ProductId,
                SellerUserName = a.FirstOrDefault().SellerUserName
            }).ToList();
            return bids;
        }

        public async Task<Bid> GetWinnerBid(string bidId)
        {
            var bids = await GetBidsByAuctionId(bidId);
            return bids.OrderByDescending(a => a.Price).FirstOrDefault();
        }

        public async Task SendBid(Bid bid)
        {
            await _context.Bids.InsertOneAsync(bid);
        }
    }
}
