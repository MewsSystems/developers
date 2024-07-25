namespace Provider.ProviderAttribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class ProviderTypeAttribute : Attribute
    {
        public ProviderTypeAttribute(string providerType)
        {
            ProviderType = providerType;
        }

        public string ProviderType { get; }
    }
}
