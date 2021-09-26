using System;
using System.Collections.Generic;
using StructureMap;
using AutoMapper;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Utilities.AutoMapper;
using ExchangeRateUpdater.Utilities.StructureMap;
using ExchangeRateUpdater.Utilities.Extensions;
using ExchangeRateUpdater.Mappings;

namespace ExchangeRateUpdater
{
    public sealed class ExchangeRateUpdaterRegistry : Registry
    {
        public ExchangeRateUpdaterRegistry()
        {
            For<IExchangeRateProvider>().Use<ExchangeRateProvider>();
            For<AutoMapperConfigurationProfile>().Singleton().Use(new AutoMapperConfigurationProfile());
             
            For<IRegisterAutoMapper>().Add<CnbExRatesToExRatesMapping>();

            For<IMapper>().Singleton().Use(ctx => createMapper(ctx.GetAllInstances<IRegisterAutoMapper>(),
                ctx.GetInstance<AutoMapperConfigurationProfile>()));
        }

        private static IMapper createMapper(IEnumerable<IRegisterAutoMapper> automapperRegistries, AutoMapperConfigurationProfile profile)
        {
            automapperRegistries.ForEach(amr => amr.Register());

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(profile));

            return mapperConfiguration.CreateMapper();
        }
    }
}
