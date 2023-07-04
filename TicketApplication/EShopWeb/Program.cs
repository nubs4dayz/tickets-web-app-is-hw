using EShop.Domain;
using EShop.Domain.Identity;
using EShop.Repository;
using EShop.Repository.Implementation;
using EShop.Repository.Interface;
using EShop.Service;
using EShop.Service.Implementation;
using EShop.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<EShopApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//var emailSettings = new EmailSettings();
//var configuration = builder.Configuration;
//configuration.GetSection("EmailSettings").Bind(emailSettings);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
//builder.Services.AddScoped<EmailSettings>(es => emailSettings);
//builder.Services.AddScoped<IEmailService, EmailService>(email => new EmailService(emailSettings));
//builder.Services.AddScoped<IBackgroundEmailSender, BackgroundEmailSender>();
//builder.Services.AddHostedService<EmailScopedHostedService>();

builder.Services.AddTransient<ITicketService, EShop.Service.Implementation.TicketService>();
builder.Services.AddTransient<IShoppingCartService, EShop.Service.Implementation.ShoppingCartService>();
builder.Services.AddTransient<IOrderService, EShop.Service.Implementation.OrderService>();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
