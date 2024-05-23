using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application;
using Webshop.Application.Contracts;
using Webshop.Orderhandling.Application.Contracts.Persistence;

namespace Webshop.Orderhandling.Application
{
    public static class OrderApplicationServiceRegistration
    {
        public static IServiceCollection AddOrderApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));

            return services;
        }
    }
}