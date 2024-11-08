using ExchangeRate.Api.Configuration;

var builder = WebApplication.CreateSlimBuilder(args);
builder.ConfigureServices();

var app = builder.Build();
app.ConfigureApplication();

app.Run();