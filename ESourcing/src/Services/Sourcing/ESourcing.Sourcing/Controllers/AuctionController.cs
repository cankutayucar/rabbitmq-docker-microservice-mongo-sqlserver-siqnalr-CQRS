using AutoMapper;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.Sourcing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _rabbitmqProducer;
        public AuctionController(IAuctionRepository auctionRepository, IBidRepository bidRepository, IMapper mapper, EventBusRabbitMQProducer rabbitmqProducer)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _mapper = mapper;
            _rabbitmqProducer = rabbitmqProducer;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Auction>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
        {
            var auctions = await _auctionRepository.GetAuctions();
            return Ok(auctions);
        }
        [HttpGet("{id:length(24)}", Name = "GetAuctionById")]
        [ProducesResponseType(typeof(Auction), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Auction>> GetAuctionById(string id)
        {
            var auction = await _auctionRepository.GetAuctionById(id);
            return auction == null ? NotFound() : Ok(auction);
        }
        [HttpPost]
        [ProducesResponseType(typeof(Auction), StatusCodes.Status201Created)]
        public async Task<ActionResult<Auction>> Create([FromBody] Auction auction)
        {
            await _auctionRepository.Create(auction);
            return CreatedAtAction("GetAuctionById", new { id = auction.Id }, auction);
        }
        [HttpPut]
        [ProducesResponseType(typeof(Auction), StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] Auction auction)
        {
            var updatedResponse = await _auctionRepository.Update(auction);
            return updatedResponse ? Ok(auction) : BadRequest(auction);
        }
        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(string id)
        {
            var updatedResponse = await _auctionRepository.Delete(id);
            return updatedResponse ? Ok() : BadRequest();
        }
        [HttpPost("CompleteAuction")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CompleteAuction(string id)
        {
            var auction = await _auctionRepository.GetAuctionById(id);
            if (auction == null) return NotFound();
            if (auction is not { Status: (int)Status.Active }) return BadRequest();
            var winnerBid = await _bidRepository.GetWinnerBid(id);
            if (winnerBid == null) return NotFound();
            OrderCreateEvent orderCreateEvent = _mapper.Map<Bid, OrderCreateEvent>(winnerBid);
            orderCreateEvent.Quantity = auction.Quantity;
            auction.Status = (int)Status.Closed;
            bool updateResponse = await _auctionRepository.Update(auction);
            if (!updateResponse) return BadRequest();
            try
            {
                _rabbitmqProducer.Publish(EventBusConstants.OrderCreateQueueName, orderCreateEvent);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = 500
                };
            }
            return Accepted();
        }
        [HttpPost("TestEvent")]
        public async Task<ActionResult> TestEvent()
        {
            OrderCreateEvent orderCreateEvent = new OrderCreateEvent
            {
                AuctionId = "dummy_1",
                ProductId = "dummy_product_1",
                Price = 100,
                Quantity = 1000,
                SellerUserName = "test@test.com",
            };
            try
            {
                _rabbitmqProducer.Publish(EventBusConstants.OrderCreateQueueName, orderCreateEvent);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = 500
                };
            }
            return Accepted();
        }
    }
}
