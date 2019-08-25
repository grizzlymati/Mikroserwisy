using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.DBContext;
using OrderService.Events;
using OrderService.Events.Interfaces;
using OrderService.Models;
using OrderService.Repository;

namespace OrderService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
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

            services.Configure<AMQPOptions>(Configuration.GetSection("amqp"));
            services.AddSingleton(typeof(IEventEmitter), typeof(AMQPEventEmitter));
            services.AddDbContext<OrderContext>(o => o.UseSqlServer(Configuration.GetConnectionString("OrderDB")));
            services.AddScoped<IOrderContext>(provider => (IOrderContext)provider.GetService(typeof(OrderContext)));
            services.AddTransient(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddSingleton(typeof(ICommandEventConverter), typeof(CommandEventConverter));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IEventEmitter eventEmitter)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
