using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Webshop.Application;
using Webshop.Application.Contracts;
using Webshop.Orderhandling.Application;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Persistence;
using Webshop.Data.Persistence;
using System.ComponentModel;

namespace Webshop.Orderhandling.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // Adding logging with Serilog
            var seqUrl = Configuration.GetValue<string>("Settings:SeqLogAddress");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", "Orderhandling.API") //enrich with the tag "service" and the name of this service
                .WriteTo.Seq(seqUrl)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Webshop.Orderhandling.Api", Version = "v1" });
            });

            // Register Orderhandling services

            // Register the OrderRepository as a singleton
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddScoped<DataContext, DataContext>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));
            services.AddOrderhandlingApplicationServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Webshop.Orderhandling.Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //add serilog
            loggerFactory.AddSerilog();
        }
    }
}
