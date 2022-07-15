using Api_cc.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);



AppSettings.Init(builder.Configuration);


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = AppSettings.WebApiSettings.AuthorityUrl;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateAudience = false
            };
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScore", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", AppSettings.WebApiSettings.Scope);
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //Ìí¼Ó²å¼þ

app.UseAuthorization();
/*
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers()
        .RequireAuthorization("ApiScore");
});
*/

app.MapControllers().RequireAuthorization("ApiScore");


app.Run();
