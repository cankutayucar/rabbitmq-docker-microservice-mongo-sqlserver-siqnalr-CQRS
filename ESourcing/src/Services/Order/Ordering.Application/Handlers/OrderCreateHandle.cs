using AutoMapper;
using MediatR;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;
using Ordering.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class OrderCreateHandle : IRequestHandler<OrderCreateCommand, OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderCreateHandle(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
        {
            return _mapper.Map<OrderResponse>(await _orderRepository.AddAsync(_mapper.Map<Order>(request)));
        }
    }
}
