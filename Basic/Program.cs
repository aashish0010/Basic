using Basic.Application.Function;
using Basic.Application.Service;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Dapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailSettings"));
DbConn.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSignalR();
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

app.UseAuthorization();
app.MapHub<ChatHub>("/chatHub");
app.MapHub<NotificationHub>("/notify");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
