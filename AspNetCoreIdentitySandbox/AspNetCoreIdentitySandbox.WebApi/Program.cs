using AspNetCoreIdentitySandbox.WebApi;
using AspNetCoreIdentitySandbox.WebApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddAuthorization();  // Add authorization policy services.
builder.Services.AddIdentityApiEndpoints<IdentityUser>()  // *Add Identity services to support Identity APIs.
    .AddEntityFrameworkStores<ApplicationDbContext>();
// Configure email confirmation.
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});
// [Optional] Customize emails sent.
builder.Services.AddTransient<IEmailSender, EmailSender>();

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

app.MapIdentityApi<IdentityUser>();  // Map Identity routes.

// Logout endpoint for cookie-based authentication. Note: For token-based
//   authentication, the client can simply delete the stored token.
app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager) =>
{
    await signInManager.SignOutAsync();  // Remove authentication-related cookies.
    return Results.Ok();
}).RequireAuthorization();

app.MapControllers();

app.Run();
