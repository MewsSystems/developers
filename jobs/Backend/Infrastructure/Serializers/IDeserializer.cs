namespace Infrastructure.Serializers
{
    public interface IDeserializer<out T>
    {
        public T Deserialize(string input);
    }
}