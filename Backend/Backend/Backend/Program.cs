using BookLibrary.Auth.Model;
using BookLibrary.Data;
using BookLibrary.Repositories;
using BookLibrary.Repositories.Interfaces;
using BookLibrary.Services;
using BookLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Database context
builder.Services.AddDbContext<BookLibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<BookLibraryDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? "YourSecretKeyHere");
builder.Services.AddAuthentication(options =>
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
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

// Register repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

// Register services
builder.Services.AddScoped<ITokenService, TokenService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Create database and apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookLibraryDbContext>();
    dbContext.Database.Migrate();
    // Seed admin user
    await BookLibrary.Data.IdentitySeeder.SeedAdminUserAsync(scope.ServiceProvider);
}

app.Run();