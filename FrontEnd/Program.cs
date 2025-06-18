using FrontEnd.Components;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Licensing;
using Syncfusion.Blazor;
using TaskTrackPro.Backend.DataAccess;
using TaskTrackPro.Backend.DataAccess.repositories;
using TaskTrackPro.Backend.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<MemoryDB>();

builder.Services.AddDbContextFactory<SqlContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        providerOptions => providerOptions.EnableRetryOnFailure())
);
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<ProyectoRepository>();
builder.Services.AddScoped<TareaRepository>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ProyectoService>();
builder.Services.AddScoped<TareaService>();
builder.Services.AddSyncfusionBlazor();
SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF1cWWhOYVJxWmFZfVtgfV9CZVZQQGY/P1ZhSXxWdkNiXn1fdHFQTmJVV0B9XUs=");




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