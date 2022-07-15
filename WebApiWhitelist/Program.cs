//https://blog.csdn.net/zls365365/article/details/122738861
using WebApiWhitelist.Config;

var builder = WebApplication.CreateBuilder(args);

AppSettings.Init(builder.Configuration);

// Add services to the container.

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ClientIpCheckActionFilter>(container =>
{
    var loggerFactory = container.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();
   

    return new ClientIpCheckActionFilter(AppSettings.WebApiSettings.IpWhiteList, logger);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<RealIpMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
