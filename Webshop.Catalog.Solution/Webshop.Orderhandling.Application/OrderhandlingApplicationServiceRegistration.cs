using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Webshop.Application;
using Webshop.Application.Contracts;

namespace Webshop.Orderhandling.Application
{
    public static class OrderhandlingApplicationServiceRegistration
    {
        public static IServiceCollection AddOrderhandlingApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));
            return services;
        }
    }
}
