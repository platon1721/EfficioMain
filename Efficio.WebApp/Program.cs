using System.Text;
using Asp.Versioning;
using Base.Contracts;
using Base.Domain;
using Efficio.BLL;
using Efficio.BLL.Contracts;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF;
using Efficio.DAL.EF.Repositories;
using Efficio.Domain.Identity;
using Efficio.WebApp.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ===================== Database =====================
builder.Services.AddDbContext<EfficioDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsql => npgsql.MigrationsAssembly("Efficio.DAL.EF")));

// ===================== Identity =====================
builder.Services
    .AddIdentity<AppUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<EfficioDbContext>()
    .AddDefaultTokenProviders();

// ===================== JWT =====================
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

// ===================== Base services =====================
builder.Services.AddScoped<IClock, SystemClock>();
builder.Services.AddScoped<ITokenService, TokenService>();

// IUserContext — resolved from HttpContext claims per request
builder.Services.AddScoped<IUserContext>(sp =>
{
    var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
    return new HttpUserContext(httpContext);
});

builder.Services.AddHttpContextAccessor();

// ===================== DAL — UOW =====================
builder.Services.AddScoped<IEfficioUOW, EfficioUOW>();

// ===================== BLL =====================
builder.Services.AddScoped<IEfficioBLL, EfficioBLL>();

// ===================== API Versioning =====================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ===================== Controllers =====================
builder.Services.AddControllers();

// ===================== CORS =====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsAllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ===================== Swagger =====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Efficio API",
        Version = "v1",
        Description = "Efficio platform REST API"
    });

    // JWT bearer in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ===================== Build =====================
var app = builder.Build();

// ===================== Pipeline =====================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsAllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


// ===================== HttpUserContext =====================
// Resolves user info from JWT claims for repository scope filtering

public class HttpUserContext : IUserContext
{
    private readonly HttpContext? _httpContext;

    public HttpUserContext(HttpContext? httpContext)
    {
        _httpContext = httpContext;
    }

    public Guid UserId =>
        Guid.TryParse(_httpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id : Guid.Empty;

    public Guid? TenantRootDepartmentId =>
        Guid.TryParse(_httpContext?.Request.Headers["X-Tenant-Id"].FirstOrDefault(), out var tenantId)
            ? tenantId : null;

    public Guid? DepartmentId =>
        Guid.TryParse(_httpContext?.Request.Headers["X-Department-Id"].FirstOrDefault(), out var deptId)
            ? deptId : null;

    public bool IsAuthenticated => _httpContext?.User.Identity?.IsAuthenticated == true;

    public bool IsPlatformAdmin =>
        _httpContext?.User.FindFirst("IsPlatformAdmin")?.Value == "true";
}