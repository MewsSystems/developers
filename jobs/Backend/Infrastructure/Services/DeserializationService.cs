using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class DeserializationService : IDeserializationService
    {
        public  T Deserialize<T>(string value)
        {
            using (TextReader reader = new StringReader(value))
            {
                try
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }
    }
}
