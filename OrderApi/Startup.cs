using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderApi.Data;
using OrderApi.Models;

namespace OrderApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //testing git
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // In-memory database:
            services.AddDbContext<OrderApiContext>(opt => opt.UseInMemoryDatabase("OrdersDb"));
            services.AddDbContext<CustomerAPIContext>(opt => opt.UseInMemoryDatabase("CustomersDB"));
            // Register repositories for dependency injection
            services.AddScoped<IRepository<Order>, OrderRepository>();
            services.AddScoped<IRepository<Customer>, CustomerRepository>();
            // Register database initializer for dependency injection

            services.AddTransient<IDbInitializer, DbInitializer>();
            services.AddSingleton<IHostedService, UpdateCreditStandingJob>();
            services.AddSingleton<IHostedService, UpdateUnpaidBills>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            // Initialize the database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                // Initialize the database
                var services = scope.ServiceProvider;
                var dbContext = services.GetService<OrderApiContext>();
                var customerContext = services.GetService<CustomerAPIContext>();
                var dbInitializer = services.GetService<IDbInitializer>();
                dbInitializer.Initialize(dbContext);
                dbInitializer.Initialize(customerContext);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
