
using EmployeeManagement.API.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<EmployeeDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("EmployeeDb") ?? "Data Source=employees.db"));
            builder.Services.AddCors(options => options.AddPolicy("Frontend", policy =>
                policy.WithOrigins(
                    "http://localhost:4200",
                    "https://alekhya8790.github.io")
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<EmployeeDbContext>().Database.EnsureCreated();
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            if (!app.Environment.IsProduction())
            {
                app.UseHttpsRedirection();
            }
            app.UseCors("Frontend");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
