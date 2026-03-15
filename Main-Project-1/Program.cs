using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MainProject1;
using MainProject1.Helpers;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SmartHome API",
        Version = "v1"
    });

    // JWT Authentication for Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter token like: Bearer {your JWT token}"
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
            new string[] {}
        }
    });
});


// Register DbContext
builder.Services.AddDbContext<SustainabilityDbContext>(options =>
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
)
);

// JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services.AddAuthentication(options =>
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
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// CORS — allow Angular dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ---- Seed default reference data ----
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SustainabilityDbContext>();
    db.Database.EnsureCreated();

    // Seed default User
    if (!db.Users.Any())
    {
        db.Users.Add(new User
        {
            Email        = "admin@smarthome.com",
            PasswordHash = PasswordHelper.Hash("admin123"),
            Name         = "Admin",
            CreatedAt    = DateTime.UtcNow
        });
        db.SaveChanges();
    }
    else
    {
        // Re-hash any plain-text passwords from old seed
        var usersWithPlainText = db.Users.ToList().Where(u => !PasswordHelper.IsHashed(u.PasswordHash));
        foreach (var u in usersWithPlainText)
            u.PasswordHash = PasswordHelper.Hash(u.PasswordHash);
        db.SaveChanges();
    }

    // Seed default Home (linked to first user)
    if (!db.Homes.Any())
    {
        var user = db.Users.First();
        db.Homes.Add(new Home
        {
            UserId    = user.Id,
            Name      = "My Home",
            Address   = "123 Smart Street",
            CreatedAt = DateTime.UtcNow
        });
        db.SaveChanges();
    }

    // Seed default Appliance Types
    if (!db.ApplianceTypes.Any())
    {
        db.ApplianceTypes.AddRange(
            new ApplianceType { Name = "Air Conditioner",  Category = "HVAC",         AvgEnergyRating = 3.5, AvgWaterRating = 0 },
            new ApplianceType { Name = "Refrigerator",     Category = "Kitchen",      AvgEnergyRating = 1.5, AvgWaterRating = 0 },
            new ApplianceType { Name = "Washing Machine",  Category = "Laundry",      AvgEnergyRating = 2.0, AvgWaterRating = 60 },
            new ApplianceType { Name = "Television",       Category = "Entertainment",AvgEnergyRating = 0.4, AvgWaterRating = 0 },
            new ApplianceType { Name = "Dishwasher",       Category = "Kitchen",      AvgEnergyRating = 1.8, AvgWaterRating = 15 },
            new ApplianceType { Name = "Water Heater",     Category = "Plumbing",     AvgEnergyRating = 4.0, AvgWaterRating = 80 },
            new ApplianceType { Name = "Smart Light",      Category = "Lighting",     AvgEnergyRating = 0.01,AvgWaterRating = 0 },
            new ApplianceType { Name = "EV Charger",       Category = "Charging",     AvgEnergyRating = 7.2, AvgWaterRating = 0 }
        );
        db.SaveChanges();
    }
}

// Configure HTTP pipeline

    app.UseSwagger();
    app.UseSwaggerUI();


// Only enable HTTPS redirection in Development (skip in Production behind Render)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAngular");

app.UseAuthentication();   // ← must come before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();