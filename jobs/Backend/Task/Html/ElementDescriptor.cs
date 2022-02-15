namespace ExchangeRateUpdater.Html
{
    public record ElementDescriptor(string Name)
    {
        private string _class;

        public ElementDescriptor WithClass(string className)
        {
            _class = className;
            return this;
        }

        public string GetStartElement()
        {
            return $"<{Name} class=\"{_class}\">";
        }
        
        public string GetEndElement()
        {
            return $"</{Name}>";
        }
    }
}