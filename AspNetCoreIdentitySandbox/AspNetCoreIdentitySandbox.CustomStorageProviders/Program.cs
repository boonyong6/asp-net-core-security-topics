using AspNetCoreIdentitySandbox.CustomStorageProviders.Data;
using AspNetCoreIdentitySandbox.CustomStorageProviders.Entities;
using AspNetCoreIdentitySandbox.CustomStorageProviders.Repositories;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container...

// Add identity types
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true);

// Identity Services
builder.Services.AddTransient<IUserStore<ApplicationUser>, CustomUserStore>();
var connectionString = builder.Configuration["Azure:StorageAccount:ConnectionString"];
builder.Services.AddSingleton<TableServiceClient>(sp => new(connectionString));
builder.Services.AddTransient<IUsersTable, AzureTableStorageUsersTable>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
