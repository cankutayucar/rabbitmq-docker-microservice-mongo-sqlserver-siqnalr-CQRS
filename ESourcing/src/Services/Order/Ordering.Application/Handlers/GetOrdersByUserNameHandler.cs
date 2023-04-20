using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Handlers
{
    public class GetOrdersByUserNameHandler : IRequestHandler<GetOrdersBySellerUserNameQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public GetOrdersByUserNameHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersBySellerUserNameQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<OrderResponse>>(await _orderRepository.GetOrdersBySellerUserName(request.UserName));
        }
    }
}
