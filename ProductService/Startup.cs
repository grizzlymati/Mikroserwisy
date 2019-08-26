using ProductService.DBContext;
using ProductService.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Models;
using ProductService.Queues;
using ProductService.Events.Interfaces;
using ProductService.Queues.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using ProductService.Queues.AMQP;
using ProductService.Events;

namespace ProductService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env,IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
               .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddOptions();


            services.Configure<QueueOptions>(Configuration.GetSection("QueueOptions"));
            services.Configure<AMQPOptions>(Configuration.GetSection("amqp"));
            services.AddDbContext<ProductContext>(o => o.UseSqlServer(Configuration.GetConnectionString("ProductDB")));
            services.AddScoped<IProductContext>(provider => (IProductContext)provider.GetService(typeof(ProductContext)));

            services.AddTransient(typeof(IConnectionFactory), typeof(AMQPConnectionFactory));
            services.AddTransient(typeof(EventingBasicConsumer), typeof(AMQPEventingConsumer));

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddSingleton(typeof(IReleasedProductsDataEventProcessor), typeof(ReleasedProductsDataEventProcessor));
            services.AddSingleton(typeof(ITakenProductsDataEventProcessor), typeof(TakenProductsDataEventProcessor));
            services.AddSingleton(typeof(IReleasedProductsDataEventSubscriber), typeof(AMQPReleasedProductsDataEventSubscriber));
            services.AddSingleton(typeof(ITakenProductsDataEventSubscriber), typeof(AMQPTakenProductsDataEventSubscriber));
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IReleasedProductsDataEventProcessor releasedProductsDataEventProcessor, ITakenProductsDataEventProcessor takenProductsDataEventProcessor)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            app.UseMvc();

            releasedProductsDataEventProcessor.Start();
            takenProductsDataEventProcessor.Start();
        }
    }
}
