using EmployeeManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Data;

public class EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
}
