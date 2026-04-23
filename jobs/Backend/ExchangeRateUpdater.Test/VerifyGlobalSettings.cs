using System.Runtime.CompilerServices;

namespace ExchangeRateUpdater.Test
{
    public class VerifyGlobalSettings
    {
        [ModuleInitializer]
        public static void InitializeVerify()
        {
            VerifyHttp.Initialize();
            Recording.Start();
        }
    }
}
