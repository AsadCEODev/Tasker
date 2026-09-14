using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TMS;
using TMS.AuthStateProvider;
using TMS.ClientServices;
using TMS.ClientServices.StateUser;
using TMS.ClientServices.TimerGlobalService;
using TSM.API.AuthDeligator;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();

// Aap ki tamam Client Services
builder.Services.AddScoped<IUserClientService, UserClientService>();
builder.Services.AddScoped<ITaskClientService, TaskClientService>();
builder.Services.AddScoped<ITagsClientService, TagsClientService>();
builder.Services.AddScoped<IStatusClientService, StatusClientService>();
builder.Services.AddScoped<ISetupProjectClientService, SetupProjectClientService>();
builder.Services.AddScoped<IAssignProjectClientService, AssignProjectClientService>();
builder.Services.AddScoped<IUserAssignedTaskClientService, UserAssignedTaskClientService>();
builder.Services.AddScoped<IDepartmentsClientService, DepartmentsClientService>();
builder.Services.AddScoped<IDesignationsClientService, DesignationsClientService>();
builder.Services.AddScoped<IAppScreenClientService, AppScreenClientService>();
builder.Services.AddScoped<IAppRolesClientService, AppRolesClientService>();
builder.Services.AddSingleton<ActiveTaskTimerService>();
builder.Services.AddScoped<UserStateService>();




// Authentication aur Auth Handler
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddTransient<JwtAuthorizationHandler>();

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

// HttpClient ko JwtAuthorizationHandler ke sath configure karna
builder.Services.AddHttpClient("TaskerAPI", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl ?? builder.HostEnvironment.BaseAddress);
})
.AddHttpMessageHandler<JwtAuthorizationHandler>();

// Default HttpClient ko configure shuda client par set karna taake saari services ko token mil sakay
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("TaskerAPI"));

await builder.Build().RunAsync();