
using EmployeeManagement.API.Data;
using EmployeeManagement.API.Models;
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
                var db = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
                db.Database.EnsureCreated();

                if (!db.Employees.Any())
                {
                    db.Employees.AddRange(
                        new Employee
                        {
                            EmployeeCode = "EMP-001",
                            FirstName = "Ananya",
                            LastName = "Sharma",
                            Email = "ananya.sharma@example.com",
                            Phone = "555-0101",
                            Gender = "Female",
                            Designation = "Software Engineer",
                            DepartmentId = 1,
                            Salary = 72000,
                            DateOfJoining = new DateTime(2024, 4, 15),
                            IsActive = true
                        },
                        new Employee
                        {
                            EmployeeCode = "EMP-002",
                            FirstName = "Rahul",
                            LastName = "Patel",
                            Email = "rahul.patel@example.com",
                            Phone = "555-0102",
                            Gender = "Male",
                            Designation = "Product Manager",
                            DepartmentId = 2,
                            Salary = 88000,
                            DateOfJoining = new DateTime(2023, 9, 1),
                            IsActive = true
                        });
                    db.SaveChanges();
                }
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
