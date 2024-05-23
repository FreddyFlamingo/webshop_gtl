using AutoMapper;
using Webshop.Orderhandling.Application.Features.Order.Commands.CreateOrder;
using Webshop.Orderhandling.Application.Features.Order.Commands.UpdateOrder;
using Webshop.Orderhandling.Application.Features.Order.Dtos;
using Webshop.Orderhandling.Application.Features.Order.Requests;
using Webshop.Orderhandling.Domain.AggregateRoots;

namespace Webshop.Orderhandling.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();

            CreateMap<Order, CreateOrderRequest>().ReverseMap();
            CreateMap<OrderItem, CreateOrderRequest.CreateOrderItem>().ReverseMap();

            CreateMap<Order, UpdateOrderRequest>().ReverseMap();
            CreateMap<OrderItem, UpdateOrderRequest.UpdateOrderItem>().ReverseMap();

            CreateMap<CreateOrderRequest, CreateOrderCommand>()
              .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<CreateOrderRequest.CreateOrderItem, CreateOrderCommand.CreateOrderItem>();

            CreateMap<UpdateOrderRequest, UpdateOrderCommand>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<UpdateOrderRequest.UpdateOrderItem, UpdateOrderCommand.UpdateOrderItem>();
        }
    }
}
