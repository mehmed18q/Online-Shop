using DomainModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Repository;
using Shop.API;
using Shop.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "data source=Sadeq;initial catalog=ShopDB;Trusted_Connection=True;Integrated Security=True;Encrypt=False;";
builder.Services.AddDbContext<ShopContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IHttpRequest, HttpRequestService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("AppSetting:ApiUrl").Value ?? string.Empty);
});

builder.Services.AddMvc();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(300);
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.RegisterServices();

builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
    app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "User",
    pattern: "{area:exists}/{controller=Orders}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Products}/{action=Index}/{id?}");

app.UseAuthentication();
app.UseAuthorization();
app.Run();