namespace Mews.Caching
{
    public class CustomCacheOptions
    {

        public CustomCacheOptions()
        {
            Name = string.Empty;
            DefaultAbsoluteExpirationRelativeToNow = null;
        }

        public string Name { get; set; }
        public virtual TimeSpan? DefaultAbsoluteExpirationRelativeToNow { get; set; }

    }
}
