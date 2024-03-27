using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniLoanApplication.Models;
using MiniLoanApplication.Service;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LoanDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IloanService, LoanService>();
builder.Services.AddScoped<IScheduledRepaymentService, ScheduledRepaymentService>();


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // For development, allow any origin
               .AllowAnyMethod()
               .AllowAnyHeader();
        
    });
});

builder.Services.AddControllers();

// Register the Swagger generator, defining one or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mini Loan Application API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini Loan Application API v1"));
}

app.UseHttpsRedirection();

// Use CORS policy
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

// Expose static files (for serving React app)
app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
