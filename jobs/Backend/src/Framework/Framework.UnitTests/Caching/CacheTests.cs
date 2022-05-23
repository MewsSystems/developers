using Framework.Caching;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace Framework.UnitTests.Caching
{
	public class CacheTests
	{
		[Fact]
		public void Cache_GetValue()
		{
			var expected = "Cached data";

			var mockMemoryCache = MockMemoryCacheService.GetMemoryCache(expected);
			var cache = new Cache(mockMemoryCache.Object);

			var result = cache.Get<string>(expected);

			Assert.NotNull(result);
			Assert.True(result == expected);
		}

		[Fact]
		public void Cache_Set_Without_Ttl()
		{
			var mockMemoryCache = new Mock<IMemoryCache>();
			mockMemoryCache
				.Setup(x => x.CreateEntry(It.IsAny<object>()))
				.Returns(Mock.Of<ICacheEntry>);

			var cache = new Cache(mockMemoryCache.Object);

			var result = cache.Set("key", "Cached data");

			Assert.True(result);
		}

		[Fact]
		public void Cache_Set_With_Ttl()
		{
			var mockMemoryCache = new Mock<IMemoryCache>();
			mockMemoryCache
				.Setup(x => x.CreateEntry(It.IsAny<object>()))
				.Returns(Mock.Of<ICacheEntry>);

			var cache = new Cache(mockMemoryCache.Object);
			var result = cache.Set("key", "Cached data", 5);

			Assert.True(result);
		}

		#region private members
		private static class MockMemoryCacheService
		{
			public static Mock<IMemoryCache> GetMemoryCache(object expectedValue)
			{
				var mockMemoryCache = new Mock<IMemoryCache>();
				mockMemoryCache
					.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
					.Returns(true);
				return mockMemoryCache;
			}
		}
		#endregion
	}
}
