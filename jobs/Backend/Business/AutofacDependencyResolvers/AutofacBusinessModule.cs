using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Common.Utils.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete;

namespace Business.AutofacDependencyResolvers
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region Declaring dependency injections
            builder.RegisterType<ExchangeRateProviderManager>().As<IExchangeRateProviderService>().SingleInstance();
            builder.RegisterType<ExchangeRateProvider>().As<IExchangeRateProvider>().SingleInstance();
            builder.RegisterType<ExchangeRateAccessor>().As<IExchangeRateAccessor>().SingleInstance();
            #endregion

            #region Declaring aspects
            var assembly =System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().EnableInterfaceInterceptors(new Castle.DynamicProxy.ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            }).SingleInstance();
            #endregion

        }
    }
}
