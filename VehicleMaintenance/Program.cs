using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VehicleMaintenance.Data;
using VehicleMaintenance.Extensions;
using VehicleMaintenance.Mappings;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Repositories;
using VehicleMaintenance.Repositories.Interfaces;
using VehicleMaintenance.Services.AI;
using VehicleMaintenance.Services.Auth;
using VehicleMaintenance.Services.Email;
using VehicleMaintenance.Services.Export;
using VehicleMaintenance.Services.RateLimiting;
using VehicleMaintenance.Services.Receipts;
using VehicleMaintenance.Services.Security;
using VehicleMaintenance.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using VehicleMaintenance.Services.GenralModelService;
using VehicleMaintenance.Services.GenralModelService.Interfaces;

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

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
builder.Services.AddScoped<IGeneralExpenseRepository, GeneralExpenseRepository>();
builder.Services.AddScoped<IGeneralExpenseService, GeneralExpenseService>();
builder.Services.AddScoped<IUserDrivingProfileRepository, UserDrivingProfileRepository>();
builder.Services.AddScoped<IUserDrivingProfileService, UserDrivingProfileService>();
builder.Services.AddScoped<DataSeeder>();
// Single SmtpEmailService satisfies both IEmailService (app code) and IEmailSender<User> (Identity internals).
builder.Services.AddScoped<SmtpEmailService>();
builder.Services.AddScoped<IEmailService>(sp => sp.GetRequiredService<SmtpEmailService>());
builder.Services.AddScoped<IEmailSender<User>>(sp => sp.GetRequiredService<SmtpEmailService>());
builder.Services.AddHttpClient<IGeminiService, GeminiService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(120);
});
builder.Services.AddScoped<IAiPredictionService, AiPredictionService>();
builder.Services.AddScoped<IReceiptParsingService, ReceiptParsingService>();
builder.Services.AddSingleton<IFileStorage, LocalFileStorage>();
builder.Services.AddScoped<IVehicleExportService, VehicleExportService>();
builder.Services.AddScoped<IVehicleOwnershipService, VehicleOwnershipService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.Configure<AiLimitsOptions>(builder.Configuration.GetSection("AiLimits"));
builder.Services.AddSingleton<IUserTierService, UserTierService>();
builder.Services.AddSingleton<IAiUsageLimiter, AiUsageLimiter>();


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = true; // new users must confirm via email link before login
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key is not configured."))),
    };
});

builder.Services.AddControllers();


builder.Services.AddAuthorization(options =>
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("ai", httpContext =>
    {
        var tier = httpContext.RequestServices
            .GetRequiredService<IUserTierService>()
            .Resolve(httpContext.User.FindFirstValue(ClaimTypes.Email));
        var key = httpContext.User.GetUserId()
            ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "anon";

        return tier.PerMinute <= 0
            ? RateLimitPartition.GetNoLimiter(key)
            : RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = tier.PerMinute,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });

    options.AddPolicy("login", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "anon",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste the token from POST /api/auth/login (no 'Bearer ' prefix)."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevPolicy", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5090",
            "https://localhost:7235",
            "http://localhost:5173",
            "http://localhost:4173"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


var app = builder.Build();

var forwardedHeaders = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardedHeaders.KnownNetworks.Clear();
forwardedHeaders.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedHeaders);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("DevPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

app.Run();
