using AutoMapper;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Mappers
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<Order, OrderCreateCommand>().ReverseMap();
        }
    }
}
