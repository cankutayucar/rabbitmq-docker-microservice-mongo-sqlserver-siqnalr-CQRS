using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.Sourcing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BitController : ControllerBase
    {
        private readonly IBidRepository _bidRepository;
        public BitController(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SendBid([FromBody] Bid bid)
        {
            await _bidRepository.SendBid(bid);
            return Ok();
        }
        [HttpGet("GetBidsByAuctionId")]
        [ProducesResponseType(typeof(IEnumerable<Bid>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByAuctionId(string auctionId)
        {
            var bids = await _bidRepository.GetBidsByAuctionId(auctionId);
            return Ok(bids);
        }
        [HttpGet("GetWinnerBid")]
        [ProducesResponseType(typeof(Bid), StatusCodes.Status200OK)]
        public async Task<ActionResult<Bid>> GetWinnerBid(string id)
        {

            var bids = await _bidRepository.GetWinnerBid(id);
            return Ok(bids);
        }
    }
}
