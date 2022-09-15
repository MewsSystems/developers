using ExchangeRateUpdater.Contract.Helpers;
using ExchangeRateUpdater.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterLocalization();
builder.Services.RegisterExchangeRateProvider(builder.Configuration);
builder.Services.RegisterCorsPolicy(builder.Configuration);
builder.Services.RegisterLocalServices();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<MemoryCacheHelper>();

builder.Services.AddControllers(options => { })
  .AddNewtonsoftJson(options =>
  {
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
  });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseWhen(x => !x.Request.Path.Value!.StartsWith("/api"), builder =>
  {
    builder.Use(async (context, next) =>
      {
        await next();
        if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
        {
          context.Request.Path = "/index.html";
          await next();
        }
      })
      .UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } })
      .UseStaticFiles();
  });
}
else
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseLocalization();
app.UseCustomExceptionHandler();
app.UseCorsPolicy();

// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Strict-Transport-Security
app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();