using AutoMapper;
using ESourcing.Sourcing.Entities;
using EventBusRabbitMQ.Events;

namespace ESourcing.Sourcing.Mappings
{
    public class SorcingMapping : Profile
    {
        public SorcingMapping()
        {
            CreateMap<OrderCreateEvent,Bid>().ReverseMap();
        }
    }
}
