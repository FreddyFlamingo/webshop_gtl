using AutoMapper;
using Webshop.Orderhandling.Application.Features.Order.Commands.CreateOrder;
using Webshop.Orderhandling.Application.Features.Order.Commands.UpdateOrder;
using Webshop.Orderhandling.Application.Features.Order.Dtos;
using Webshop.Orderhandling.Application.Features.Order.Requests;
using Webshop.Orderhandling.Domain.AggregateRoots;
using Webshop.Catalog.Application.Features.Product.Dtos;
using Webshop.Catalog.Domain.AggregateRoots;

namespace Webshop.Orderhandling.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            CreateMap<OrderDto, Order>();

            CreateMap<Order, CreateOrderRequest>().ReverseMap();

            CreateMap<Order, UpdateOrderRequest>().ReverseMap();

            CreateMap<CreateOrderRequest, CreateOrderCommand>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            CreateMap<UpdateOrderRequest, UpdateOrderCommand>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            // Add the mapping for Product and ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU))
                .ForMember(dest => dest.AmountInStock, opt => opt.MapFrom(src => src.AmountInStock))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.MinStock, opt => opt.MapFrom(src => src.MinStock))
                .ReverseMap();
        }
    }
}
