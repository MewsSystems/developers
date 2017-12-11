using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Strings;

namespace ExchangeRateUpdater.Core
{
	public class RestartProgramStep : IProgramStep {
		public async Task<bool> RunAsync(ProgramContext context) {
			context.WriteMessage(ConsoleMessageResource.StartOverQuestionMessage);

			var key = Console.ReadKey();

			if(key.Key == ConsoleKey.Y) {
				context.Reset();
			} else {
				context.Quit();
			}
			
			return false;
		}
	}
}
