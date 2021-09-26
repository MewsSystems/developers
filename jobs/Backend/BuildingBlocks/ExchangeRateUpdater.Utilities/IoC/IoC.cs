using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Utilities.IoC
{
    public static class IoC
    {
        public static void Init()
        {
            Container = new Container();
        }

        public static void AddRegistry(Registry registry)
        {
            Container?.Configure(c => c.AddRegistry(registry));
        }

        public static IContainer Container { get; private set; }
    }
}
