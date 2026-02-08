using BlazorApp3.Client.Data.implements;
using BlazorApp3.Client.Data.Inteface;
using BlazorApp3.Client.Data.Options;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

string? httpclientName = builder.Configuration["TodoHttpClientName"];

//ArgumentException.ThrowIfNullOrEmpty(httpclientName);

 /*builder.Services.AddHttpClient(
    httpclientName,
    client =>
    {
        client.BaseAddress = new Uri("https://localhost7052/");

        client.DefaultRequestHeaders.UserAgent.ParseAdd("dotnet-docs");
    });
*/
builder.Services.AddScoped(typeof(ISend<>), typeof(Sender<>));

/*
var services = new ServiceCollection();

services.AddHttpClient();

var serviceprovider = services.BuildServiceProvider();

var httpclientfactory = serviceprovider.GetService<IHttpClientFactory>();

var httpclient = httpclientfactory?.CreateClient();

httpclient.BaseAddress = new Uri("https://localhost7052");

httpclient.DefaultRequestHeaders.UserAgent.ParseAdd("");

*/

await builder.Build().RunAsync();


