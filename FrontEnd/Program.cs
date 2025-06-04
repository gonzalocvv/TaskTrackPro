using FrontEnd.Components;
using Servicios;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Licensing;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<MemoryDB>();

builder.Services.AddDbContextFactory<SqlContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        providerOptions => providerOptions.EnableRetryOnFailure())
);
builder.Services.AddSingleton<UsuarioService>();
builder.Services.AddSingleton<ProyectoService>();
builder.Services.AddSyncfusionBlazor();
SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF1cWWhPYVJyWmFZfVtgdV9GYlZUQmYuP1ZhSXxWdkBiXH9fcXVWQGdVUEV9XUs=");

builder.Services.AddSingleton<TareaService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();