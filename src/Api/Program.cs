using System.Reflection;
using System.Text;
using API.Models;
using Core.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sieve.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
builder.Services.AddControllers();

// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IAuthService, AuthService>();

// builder
//     .Services.AddIdentity<ApplicationUser, IdentityRole>()
//     .AddEntityFrameworkStores<BlogDbContext>()
//     .AddDefaultTokenProviders();
//
// builder.Services.Configure<IdentityOptions>(options =>
// {
//     // Customize or remove password requirements
//     options.Password.RequireDigit = false; // Remove requirement for a digit
//     options.Password.RequireLowercase = false; // Remove requirement for a lowercase letter
//     options.Password.RequireUppercase = false; // Remove requirement for an uppercase letter
//     options.Password.RequireNonAlphanumeric = false; // Remove requirement for a non-alphanumeric character (symbol)
//     options.Password.RequiredLength = 6; // Set a custom minimum length for the password
//     options.Password.RequiredUniqueChars = 0; // Remove requirement for unique characters
// });
builder
    .Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Ensure that the email is unique
        options.User.RequireUniqueEmail = true;

        // Customize or remove password requirements
        options.Password.RequireDigit = false; // Remove requirement for a digit
        options.Password.RequireLowercase = false; // Remove requirement for a lowercase letter
        options.Password.RequireUppercase = false; // Remove requirement for an uppercase letter
        options.Password.RequireNonAlphanumeric = false; // Remove requirement for a non-alphanumeric character (symbol)
        options.Password.RequiredLength = 6; // Set a custom minimum length for the password
        options.Password.RequiredUniqueChars = 0; // Remove requirement for unique characters
    })
    .AddEntityFrameworkStores<BlogDbContext>()
    .AddDefaultTokenProviders();

// Add DbContext with SQLite
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        }
    );
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        };
    });

// builder.Services.AddControllers(options => options.Filters
//   .Add(typeof(RegisterValidator)));


// builder.Services.Add(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());


builder.Services.AddScoped<SieveProcessor>();

// Add FluentValidation

builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

// builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

// builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
//
// builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddFluentValidationClientsideAdapters();
// builder.Services.AddValidatorsFromAssembly(typeof(RegisterValidator).Assembly);

// builder
//     .Services.AddControllers()
//     .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterValidator>());

// builder.Services.AddSieve();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Add this line

app.MapControllers();

var summaries = new[]
{
    "Freezing",
    "Bracing",
    "Chilly",
    "Cool",
    "Mild",
    "Warm",
    "Balmy",
    "Hot",
    "Sweltering",
    "Scorching",
};

app.MapGet(
        "/weatherforecast",
        () =>
        {
            var forecast = Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        }
    )
    .WithName("GetWeatherForecast")
    .WithOpenApi();

// app.Run();
app.Run("http://localhost:8000"); // Change port here

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
