using System;
using System.Web;
using Accounts;
using Accounts.Core.Helpers;
using Accounts.Core.Repositories;
using Accounts.Core.Repositories.Interfaces;
using Accounts.Database.DataAccess;
using Accounts.Database.DataAccess.Interfaces;
using Accounts.ModelBuilders;
using Accounts.Models;
using AutoMapper;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using IPaymentModelBuilder = Accounts.Core.ModelBuilders.IPaymentModelBuilder;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Accounts
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IClock>().To<Clock>();
            kernel.Bind<IPaymentModelBuilder>().To<Core.ModelBuilders.PaymentModelBuilder>();

            kernel.Bind<IPaymentRepository>().To<PaymentRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IDbConnectionFactory>().To<SqlConnectionFactory>();
            kernel.Bind<IPaymentModelFactory>().To<PaymentModelFactory>();
            kernel.Bind<IPaymentHelper>().To<PaymentHelper>();
            kernel.Bind<IConnectionString>().To<ConnectionString>()
                .WithConstructorArgument("connectionString", "AccountDb");

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Core.Models.PaymentModel, PaymentModel>();
                cfg.CreateMap<Core.Models.UserModel, UserModel>();
            });

            kernel.Bind<IMapper>().ToConstant(mapperConfiguration.CreateMapper());
        }        
    }
}
