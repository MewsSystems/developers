using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IContainer Register()
        {
            // Microsoft DI
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            // Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);

            var thisAssembly = Assembly.GetExecutingAssembly();
            containerBuilder.RegisterAssemblyTypes(thisAssembly)
                .Where(t => t.Name.EndsWith("Provider"))
                .AsImplementedInterfaces();

            return containerBuilder.Build();
        }
    }
}
