using Castle.DynamicProxy;

namespace ExchangeRateUpdaterTests
{
	internal class MethodInterceptor : IInterceptor
	{
		private string methodName;
		private readonly Action<IInvocation> onIntercept;

		public MethodInterceptor(string methodName, Action<IInvocation> onIntercept)
		{
			this.methodName = methodName;
			this.onIntercept = onIntercept;
		}

		public void Intercept(IInvocation invocation)
		{
			if (invocation.Method.Name == methodName)
			{
				onIntercept(invocation);
			}

			invocation.Proceed();
		}
	}
}
