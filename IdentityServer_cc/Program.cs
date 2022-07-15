//https://identityserver4docs.readthedocs.io/zh_CN/latest/quickstarts/1_client_credentials.html
using IdentityServer_cc.Configs;

var builder = WebApplication.CreateBuilder(args);


//Add functionality to inject IOption<T>
AppSettings.Init(builder.Configuration);

// Add services to the container.
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()     //这仅适用于没有证书可以使用的开发场景。
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
///builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    ///app.UseSwagger();
    ///app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseIdentityServer();//添加中间件

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
