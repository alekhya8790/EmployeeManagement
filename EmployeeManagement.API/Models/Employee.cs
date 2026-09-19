using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    [Required, MaxLength(30)]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Gender { get; set; } = "Male";

    [MaxLength(100)]
    public string Designation { get; set; } = string.Empty;

    public int DepartmentId { get; set; } = 1;

    public decimal Salary { get; set; }

    public DateTime DateOfJoining { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}
