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
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Products));


            CreateMap<OrderDto, Order>();

            CreateMap<Order, CreateOrderRequest>().ReverseMap();

            CreateMap<Order, UpdateOrderRequest>().ReverseMap();

            CreateMap<CreateOrderRequest, CreateOrderCommand>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            CreateMap<UpdateOrderRequest, UpdateOrderCommand>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
        }
    }
}
