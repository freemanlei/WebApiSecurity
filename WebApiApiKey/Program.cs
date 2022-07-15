//https://www.cnblogs.com/chenxizhang/p/aspnet-core6-apikey-authorization.html
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var API_KEY_NAME = "ApiKey";
var API_KEY_KEY = "abc";
var API_KEY_SEGMENTS = "/swagger";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen ����������Ա���Swagger ��ҳ���������ApiKey���е��ԡ�
builder.Services.AddSwaggerGen((options) =>
{
    options.AddSecurityDefinition(API_KEY_NAME, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In =ParameterLocation.Header,
        Name = API_KEY_NAME
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = API_KEY_NAME
                }
            },
            new String[]{}
        }
    });
});

var app = builder.Build();

//��һ���м����������֤ApiKey
app.Use(async (context, next) =>
{
    var found = context.Request.Headers.TryGetValue(API_KEY_NAME, out var key);

    if(context.Request.Path.StartsWithSegments(API_KEY_SEGMENTS) ||(found && key == API_KEY_KEY))
    {
        await next(context);
    }
    else
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Unauthorized");
        return;
    }

});

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
