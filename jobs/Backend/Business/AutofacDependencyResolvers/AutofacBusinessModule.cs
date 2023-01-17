using Autofac;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AutofacDependencyResolvers
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExchangeRateProviderManager>().As<IExchangeRateProviderService>().SingleInstance();
            builder.RegisterType<ExchangeRateProvider>().As<IExchangeRateProvider>().SingleInstance();

        }
    }
}
