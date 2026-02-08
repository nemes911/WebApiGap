

using API_GAI.DbServices.DefaultCommand.Implements;
using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Data.Auth;
using API_GAI.DbServices.SRC.Models;
using API_GAI.Settings;
using WebApiGap.DbServices.DefaultCommand.Implements;
using WebApiGap.DbServices.DefaultCommand.Interface;
using WebApiGap.DbServices.PostgresFactory;
using WebApiGap.Settings.Audinthification;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

–аскоментить подключеное проксирование 
*/
builder.Services.AddSingleton<PostgresContext>();

builder.Services.AddScoped<PostgresContextFactory>();

builder.Services.AddScoped<IUser, User>();

builder.Services.Configure<AppiSettings>(builder.Configuration.GetSection("AppiSettings"));

builder.Services.AddScoped(typeof(IDefaultDB<>), typeof(DefaultDb<>));

builder.Services.AddSingleton<Authzorization>();

/* builder.Services.AddSingleton<Session>(); раскоментить и доделать сессию  */



builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
