using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.Mappings;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Services;
using VehicleMaintenance.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleComponentService, VehicleComponentService>();
builder.Services.AddScoped<IFuelEntryService, FuelEntryService>();
builder.Services.AddScoped<IMaintenanceRecordService, MaintenanceRecordService>();
builder.Services.AddScoped<IMaintenanceRecordComponentService, MaintenanceRecordComponentService>();
builder.Services.AddScoped<IPredictionService, PredictionService>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddSingleton<IEmailSender<User>, NoOpEmailSender<User>>();


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
});

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevPolicy", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5090",
            "https://localhost:7235",
            "http://localhost:5173"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DevPolicy");
app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

app.Run();
