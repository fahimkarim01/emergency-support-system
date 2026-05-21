using EmergencySupport.Data;
using EmergencySupport.Repos;
using EmergencySupport.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EsupportDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("EsupportDb")));

builder.Services.AddScoped<UsersRepo>();
builder.Services.AddScoped<EmergencyRequestsRepo>();
builder.Services.AddScoped<RespondersRepo>();
builder.Services.AddScoped<AssignmentsRepo>();
builder.Services.AddScoped<NotificationsRepo>();
builder.Services.AddScoped<FeedbackRepo>();
builder.Services.AddScoped<ReportsLogsRepo>();
builder.Services.AddScoped<CurrentUserHelper>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication("EsAuth")
.AddCookie("EsAuth", opt =>
{
    opt.LoginPath = "/Auth/Login";
    opt.AccessDeniedPath = "/Auth/Denied";
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
