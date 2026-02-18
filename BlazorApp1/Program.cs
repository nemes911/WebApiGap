using BlazorApp1.Components;
using BlazorApp1.Http.Implements;
using BlazorApp1.Http.Service;
using BlazorApp1.SettingsApi;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("BlazorClient", c =>
{
    c.BaseAddress = new Uri("https://127.0.0.1:7011/test-auth");
    c.DefaultRequestHeaders.Add("X-session-Id", "");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();

    using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

    store.Open(OpenFlags.ReadOnly);

    var cert = store.Certificates
        .Find(X509FindType.FindBySubjectName, "localhost", false)
        .OfType<X509Certificate2>()
        .FirstOrDefault();

    store.Close();
    if(cert != null)
    {
        handler.ClientCertificates.Add(cert);
    }
    //Devolepment delete by prod
    handler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return handler;
} );


builder.Services.AddScoped<SessionService>();

builder.Services.AddTransient(typeof(ISender<>), typeof(Sender<>));

builder.Services.AddScoped<Key>();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
    
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
