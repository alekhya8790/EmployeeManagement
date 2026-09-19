using EmployeeManagement.API.Data;
using EmployeeManagement.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(EmployeeDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        return Ok(await db.Employees.AsNoTracking().OrderBy(employee => employee.EmployeeId).ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await db.Employees.FindAsync(id);
        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
    {
        employee.EmployeeId = 0;
        db.Employees.Add(employee);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
    {
        if (id != employee.EmployeeId)
        {
            employee.EmployeeId = id;
        }

        if (!await db.Employees.AnyAsync(existing => existing.EmployeeId == id))
        {
            return NotFound();
        }

        db.Entry(employee).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await db.Employees.FindAsync(id);
        if (employee is null)
        {
            return NotFound();
        }

        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
