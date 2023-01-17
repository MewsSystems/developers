using System.Xml.Serialization;


public abstract class BankServiceBase
{
    private readonly HttpClient _client;

    protected BankServiceBase(HttpClient client) 
        => _client = client;

    protected async Task<TResp?> Get<TResp>(string urlPath)
    {
        var response = await _client.GetAsync(urlPath);
        response.EnsureSuccessStatusCode();
        return DeserializeXml<TResp>(await response.Content.ReadAsStreamAsync().ConfigureAwait(false));
    }

    //PUT
    
    //POST

    private static T DeserializeXml<T>(Stream data)
    {
        using var reader = new StreamReader(data);
        return (T)new XmlSerializer(typeof(T))
            .Deserialize(reader)!;
    }
}