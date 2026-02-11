

using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Data.Auth;
using API_GAI.DbServices.SRC.Models;
using API_GAI.Settings;
using WebApiGap.DbServices.DefaultCommand.Implements;
using WebApiGap.DbServices.DefaultCommand.Interface;
using WebApiGap.DbServices.PostgresFactory;
using WebApiGap.Session.Service;
using WebApiGap.Session.ServiceSession;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

–аскоментить подключеное проксирование 
*/

builder.Services.AddScoped<PostgresContextFactory>();

builder.Services.AddScoped<IUser, User>();

builder.Services.Configure<AppiSettings>(builder.Configuration.GetSection("AppiSettings"));

builder.Services.AddScoped(typeof(IDefaultDB<>), typeof(DefaultDb<>));

builder.Services.AddSingleton<Authzorization>();

builder.Services.AddSingleton<ISessionStorenterface, InMemoryStore>();



/* builder.Services.AddSingleton<Session>(); раскоментить и доделать сессию  */



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = " My API", Version = "v1" });

    c.AddSecurityDefinition("X-Session-Id", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-Session-Id",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "Session Id for swagger debug"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "X-Session-Id",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "X-Session-Id"
                },
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Use(async (context, next) =>
{
    var store = context.RequestServices.GetRequiredService<ISessionStorenterface>();

    var user = context.RequestServices.GetRequiredService<IUser>();

    if(context.Request.Headers.TryGetValue("X-Session-Id", out var id))
    {
        var username = store.GetUser(id);

        var passwword = store.GetPassword(id);

        if (!string.IsNullOrEmpty(username) && (!string.IsNullOrEmpty(passwword)))
        {
            user.name = username;
            user.password = passwword;
        }
    }

    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
