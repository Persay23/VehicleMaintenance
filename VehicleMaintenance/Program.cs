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
using VehicleMaintenance.Services;
using VehicleMaintenance.Services.AI;
using VehicleMaintenance.Services.Auth;
using VehicleMaintenance.Services.Export;
using VehicleMaintenance.Services.Interfaces;
using VehicleMaintenance.Services.RateLimiting;
using VehicleMaintenance.Services.Receipts;
using VehicleMaintenance.Services.Security;
using VehicleMaintenance.Services.Storage;
using Microsoft.AspNetCore.Authorization;

// QuestPDF Community licence — free for individuals / orgs under the revenue threshold (covers this project).
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
builder.Services.AddSingleton<IEmailSender<User>, NoOpEmailSender<User>>();
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

// AI rate limiting: config-driven tiers (Regular/Premium/Max) + an in-memory daily quota.
builder.Services.Configure<AiLimitsOptions>(builder.Configuration.GetSection("AiLimits"));
builder.Services.AddSingleton<IUserTierService, UserTierService>();
builder.Services.AddSingleton<IAiUsageLimiter, AiUsageLimiter>();


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// JWT bearer is the default scheme (overrides the cookie scheme AddIdentity registers).
// The SPA sends the token in the Authorization header, so this works cross-origin and
// returns a clean 401 (no cookie redirect) on unauthenticated requests.
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

// Require an authenticated user on every endpoint by default. Public entry points
// (login / register) opt out with [AllowAnonymous]. Closes the previous hole where most
// controllers were reachable anonymously.
builder.Services.AddAuthorization(options =>
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

// Per-minute burst limiter. The "ai" policy is tier-aware (Max tier → no limit, for testing);
// the "login" policy is a per-IP brute-force guard. Rejections return 429.
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
    // Adds an "Authorize" button so you can paste a JWT and call protected endpoints from Swagger.
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
            "http://localhost:4173"   // `npm run preview` (production build / PWA testing)
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


var app = builder.Build();

// Behind App Service's reverse proxy: trust X-Forwarded-For/Proto so the app sees the real
// client IP and knows the request arrived over HTTPS (needed for correct redirects + scheme).
// KnownNetworks/Proxies cleared because the only ingress is the trusted App Service front end.
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
    // Apply any pending EF migrations on startup. Azure SQL starts empty, so this builds the
    // schema on first boot; locally it's a no-op when the DB is already up to date. Idempotent.
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

app.Run();
