using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Orderhandling.Application.Features.Order.Dtos;
using Webshop.Orderhandling.Application.Features.Order.Requests;
using Webshop.Orderhandling.Domain.AggregateRoots;

namespace Webshop.Orderhandling.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Webshop.Orderhandling.Domain.AggregateRoots.Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Order, CreateOrderRequest>().ReverseMap();
            CreateMap<OrderItem, CreateOrderRequest.CreateOrderItem>().ReverseMap();
            CreateMap<Order, UpdateOrderRequest>().ReverseMap();
            CreateMap<OrderItem, UpdateOrderRequest.UpdateOrderItem>().ReverseMap();
        }
    }
}