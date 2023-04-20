using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Application.Queries;
using Ordering.Application.Responses;

namespace ESourcing.Order.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetOrdersByUserName/{userName}")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
        {
            var orders = await _mediator.Send(new GetOrdersBySellerUserNameQuery(userName));
            if (orders.Count() == decimal.Zero) return NotFound();
            return Ok(orders);
        }
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderResponse>> OrderCreate([FromBody] OrderCreateCommand orderCreateCommand)
        {
            var response = await _mediator.Send(orderCreateCommand);
            return Ok(response);
        }
    }
}
