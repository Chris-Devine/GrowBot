using System;
using System.Web;
using GrowBot.API.App_Start;
using GrowBot.API.Factories.GrowSettings;
using GrowBot.API.Factories.GrowSettings.Interfaces;
using GrowBot.API.Helpers;
using GrowBot.API.Helpers.HelpersInterfaces;
using GrowBot.API.Repository.Repositories.GrowSettings;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof (NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof (NinjectWebCommon), "Stop")]

namespace GrowBot.API.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof (OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof (NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        ///     Creates the kernel that will manage your application.
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
        ///     Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Settings.AllowNullInjection = true;
            kernel.Bind<IGrowSettingsRepository>().To<GrowSettingsRepository>();
            kernel.Bind<IUserHelper>().To<UserHelper>();
            kernel.Bind<IGrowSettingFactory>().To<GrowSettingFactory>();
            kernel.Bind<IGrowPhaseSettingFactory>().To<GrowPhaseSettingFactory>();
            kernel.Bind<IWaterSettingFactory>().To<WaterSettingFactory>();
            kernel.Bind<ILightSettingFactory>().To<LightSettingFactory>();
            kernel.Bind<IFanSettingFactory>().To<FanSettingFactory>();
        }
    }
}