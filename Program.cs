using Microsoft.AspNetCore.Authentication.JwtBearer;  // For JWT authentication support
using Microsoft.EntityFrameworkCore;                  // For Entity Framework Core (database ORM)
using Microsoft.IdentityModel.Tokens;                 // For token validation parameters
using SportsTicketBooking.Data;                        // Your project’s data layer (DbContext)
using SportsTicketBooking.Models;                      // Your project’s data models (like User, Ticket, etc.)
using SportsTicketBooking.Services;                    // Your project’s service layer (business logic)
using System.Text;                                     // For encoding keys (like JWT key)
using Microsoft.OpenApi.Models;                        // For Swagger/OpenAPI documentation

var builder = WebApplication.CreateBuilder(args);     // Create builder to configure the application

// Configure database context with MySQL connection string from configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"), // Gets connection string from appsettings.json or environment
        new MySqlServerVersion(new Version(8, 0, 21))                    // Specifies MySQL version for compatibility
    )
);

// Register your service classes for dependency injection
builder.Services.AddScoped<AuthService>();    // AuthService handles authentication logic
builder.Services.AddScoped<TicketService>();  // TicketService handles ticket-related business logic

// Add support for controllers and customize JSON serialization options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Add custom JSON converter, e.g., UserTypeConverter, for serializing specific types
        options.JsonSerializerOptions.Converters.Add(new UserTypeConverter());
    });

// Configure CORS (Cross-Origin Resource Sharing) policy to allow frontend (React, etc.) hosted on localhost:3000 to call this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // Frontend app origin
                   .AllowAnyHeader()                     // Allow any headers
                   .AllowAnyMethod();                    // Allow all HTTP methods (GET, POST, etc.)
        });
});

// Add Swagger/OpenAPI support to generate API documentation and UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SportsTicketBooking API", Version = "v1" });
});

// Configure JWT (JSON Web Token) authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  // Use JWT bearer authentication scheme
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,               // Validate token issuer (who issued the token)
            ValidateAudience = true,             // Validate token audience (who the token is meant for)
            ValidateLifetime = true,             // Validate token expiration
            ValidateIssuerSigningKey = true,    // Validate the signing key is correct and trusted
            ValidIssuer = builder.Configuration["Jwt:Issuer"],           // Expected issuer, from config
            ValidAudience = builder.Configuration["Jwt:Audience"],       // Expected audience, from config
            IssuerSigningKey = new SymmetricSecurityKey(                 // Signing key used to validate token signature
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"] 
                    ?? throw new InvalidOperationException("Jwt:Key is not configured.")
                )
            )
        };
    });

var app = builder.Build();  // Build the app with all the configured services and middleware

// Add authentication middleware so app validates JWT tokens in incoming requests
app.UseAuthentication();

// Add authorization middleware to enforce policies and roles on endpoints
app.UseAuthorization();

// Enable CORS with the defined policy so frontend at localhost:3000 can access this API
app.UseCors("AllowLocalhost3000");

// Map controller routes so API endpoints can be accessed
app.MapControllers();

// Run the application and start listening for incoming HTTP requests
app.Run();
