using System.Diagnostics;
using System.Management;

namespace ExchangeRateUpdater.AcceptanceTests
{
    internal class ProcessHelpers
    {
        internal static void KillProcessAndChildren(int processId)
        {
            var searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessId=" + processId);
            var managementObjects = searcher.Get();
            foreach (var managementObject in managementObjects)
            {
                KillProcessAndChildren(Convert.ToInt32(managementObject["ProcessId"]));
            }

            try
            {
                Process process = Process.GetProcessById(processId);
                process.Kill();
                process.WaitForExit(); // Optionally wait for the process to exit
            }
            catch (ArgumentException)
            {
                // Process already exited
            }
            catch (InvalidOperationException)
            {
                // Process already exited
            }
        }
    }
}
