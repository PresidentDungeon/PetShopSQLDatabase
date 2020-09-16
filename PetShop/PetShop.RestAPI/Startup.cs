using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PetShop.Core.ApplicationService;
using PetShop.Core.ApplicationService.Impl;
using PetShop.Core.DomainService;
using PetShop.Core.Search;
using PetShop.Infrastructure.Data;
using PetShop.Infrastructure.SQLLite.Data;

namespace PetShop.RestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPetRepository, PetSQLRepository> ();
            services.AddScoped<IOwnerRepository, OwnerSQLRepository>();
            services.AddScoped<IPetTypeRepository, PetTypeSQLRepository>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IOwnerService, OwnerService>();
            services.AddScoped<IPetTypeService, PetTypeService>();
            services.AddScoped<IPetExchangeService, PetExchangeService>();
            services.AddScoped<ISearchEngine, SearchEngine>();
            services.AddScoped<InitStaticData>();

            services.AddControllers().AddNewtonsoftJson(options => 
            { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
              options.SerializerSettings.MaxDepth = 3; 
            });

            services.AddDbContext<PetShopContext>(opt => { opt.UseSqlite("Data Source=PetShopApp.db"); });

            services.AddSwaggerGen((options) => {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
                { 
                    Title = "Pet Shop",
                    Description = "A RestAPI for pet a pet exchange application",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetShop API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetService<PetShopContext>();
                    ctx.Database.EnsureDeleted();
                    ctx.Database.EnsureCreated();

                    InitStaticData dataInitilizer = scope.ServiceProvider.GetRequiredService<InitStaticData>();
                    dataInitilizer.InitData();
                }
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
