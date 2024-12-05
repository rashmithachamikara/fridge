using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the database provider and connection string from the configuration
var databaseProvider = builder.Configuration["DatabaseProvider"];
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the appropriate DbContext based on the database provider
if (databaseProvider == "MSSQL")
{
    builder.Services.AddDbContext<MSSQLApplicationDBContext>(options =>
        options.UseSqlServer(connectionString));
}
else if (databaseProvider == "MySQL")
{
    builder.Services.AddDbContext<MySQLApplicationDBContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}
else
{
    throw new InvalidOperationException("Unsupported database provider specified in configuration.");
}

// Register the DbContext with a scoped lifetime. Needed for dependency injection in controllers
builder.Services.AddScoped<DbContext>(serviceProvider =>
{
    if (databaseProvider == "MSSQL")
    {
        return serviceProvider.GetRequiredService<MSSQLApplicationDBContext>();
    }
    else if (databaseProvider == "MySQL")
    {
        return serviceProvider.GetRequiredService<MySQLApplicationDBContext>();
    }
    else
    {
        throw new InvalidOperationException("Unsupported database provider specified in configuration.");
    }
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Allows all origins
              .AllowAnyMethod()  // Allows all HTTP methods
              .AllowAnyHeader(); // Allows all headers
    });
});

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    // Determine the correct DbContext type and apply migrations
    if (databaseProvider == "MSSQL")
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MSSQLApplicationDBContext>();
        dbContext.Database.Migrate(); // Apply MSSQL migrations
    }
    else if (databaseProvider == "MySQL")
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MySQLApplicationDBContext>();
        dbContext.Database.Migrate(); // Apply MySQL migrations
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS policy
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
