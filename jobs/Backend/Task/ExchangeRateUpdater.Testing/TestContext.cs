using Moq.AutoMock;

namespace ExchangeRateUpdate.Testing;

public class TestContext<TSut>
	where TSut : class
{
	private TSut? _sut;

	public TestContext()
	{
		AutoMocker = new AutoMocker();
	}

	public TSut Sut
	{
		get
		{
			if (_sut is null)
			{
				_sut = BuildSut(AutoMocker);
			}

			return _sut;
		}
	}

	protected AutoMocker AutoMocker { get; }

	protected virtual TSut BuildSut(AutoMocker autoMocker) => autoMocker.CreateInstance<TSut>();
}