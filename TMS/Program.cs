using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TMS;
using TMS.ClientServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IUserClientService, UserClientService>();
builder.Services.AddScoped<ITaskClientService, TaskClientService>();
builder.Services.AddScoped<ITagsClientService, TagsClientService>();
builder.Services.AddScoped<IStatusClientService, StatusClientService>();

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

// 2. HttpClient ko configure karein
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl ?? builder.HostEnvironment.BaseAddress)
});

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
