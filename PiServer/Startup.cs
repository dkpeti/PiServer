using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiServer.Context;
using PiServer.DataManagers;

namespace PiServer
{
    public class Startup
    {
        private IWebHostEnvironment env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            if (env.IsDevelopment())
            {
                services.AddEntityFrameworkNpgsql().AddDbContext<PiDbContext>(opt => opt.UseNpgsql(Configuration["ConnectionString:PiDB"]));
            }
            else
            {
                var connString = $"Host={Configuration["ConnectionString:Host"]};Database={Configuration["ConnectionString:DB"]};Username={Configuration["ConnectionString:Username"]};Password={Configuration["ConnectionString:Password"]};Integrated Security=true;Pooling=true;";
                services.AddEntityFrameworkNpgsql().AddDbContext<PiDbContext>(opt => opt.UseNpgsql(connString));
            }
            //services.AddDbContext<PiDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:PiDB"]));

            services.AddSingleton<IrrigationServerConnection>();
            services.AddHostedService<IrrigationServerConnection>(provider => provider.GetService<IrrigationServerConnection>());
            services.AddScoped<ISzenzorManager, SzenzorManager>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Irrigation API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PiDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Irrigation API");
                c.RoutePrefix = string.Empty;
            });

            dbContext.Database.Migrate();
        }
    }
}
