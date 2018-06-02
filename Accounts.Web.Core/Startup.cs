using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Core.Helpers;
using Accounts.Core.ModelBuilders;
using Accounts.Core.Repositories;
using Accounts.Core.Repositories.Interfaces;
using Accounts.Database.DataAccess;
using Accounts.Database.DataAccess.Interfaces;
using Accounts.Web.ModelBuilders;
using Accounts.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Web.Core
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
            services.AddMvc();


            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Accounts.Core.Models.PaymentModel, PaymentModel>();
                cfg.CreateMap<PaymentModel, Accounts.Core.Models.PaymentModel>();
                cfg.CreateMap<Accounts.Core.Models.UserModel, UserModel>();
                cfg.CreateMap<UserModel, Accounts.Core.Models.UserModel>();
            });

            services.AddTransient<IMapper>((target) => mapperConfiguration.CreateMapper());
            services.AddTransient<IClock, Clock>();
            services.AddTransient<IPaymentModelBuilder, PaymentModelBuilder>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDbConnectionFactory, SqlConnectionFactory>();
            services.AddTransient<IPaymentModelFactory, PaymentModelFactory>();
            services.AddTransient<IPaymentHelper, PaymentHelper>();
            services.AddTransient<IConnectionString, ConnectionString>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }



            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
