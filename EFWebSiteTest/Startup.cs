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
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepoLayer;
using ServiceLayer;
using Domain;
using Mapper;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace EFWebSiteTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ServiceProvider = new AutofacServiceProvider(this.ApplicationContainer);

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Error;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddScoped<IBrandService,BrandService>();
            services.AddScoped<IInfoRequestService, InfoRequestService>();
            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<ProductCategoryService>();

            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IBrandRepository,BrandRepository>();
            services.AddScoped<IInfoRequestRepository,InfoRequestRepository>();
            services.AddScoped<IProductCategoryRepository,ProductCategoryRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();


            services.AddAutoMapper(typeof(InfoRequest),typeof(InfoRequestMapperConfig));
            services.AddAutoMapper(typeof(InfoRequestReply), typeof(InfoRequestMapperConfig));
            services.AddAutoMapper(typeof(Product), typeof(ProductMapperConfig));
            services.AddAutoMapper(typeof(Brand), typeof(BrandMapperConfig));




            services.AddDbContextPool<MyDbContext>(optionsBuilder => {
                string ConnectionString = Configuration.GetConnectionString("Default");
                optionsBuilder.UseSqlServer(ConnectionString).EnableSensitiveDataLogging(true);
            });




            services.AddCors(options =>
            {
                options.AddPolicy("VueCorsPolicy", builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("VueCorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}"
                );
                endpoints.MapControllers();
            });

        }
    }
}
