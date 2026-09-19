import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent implements OnInit {
  employees: any[] = [];
  filteredEmployees: any[] = [];
  searchTerm: string = '';
  isEditMode: boolean = false;
  editingId: number | null = null;

  apiUrl = 'https://employee-management-api-production-ea40.up.railway.app/api/Employees';

  newEmp: any = {
    employeeCode: '',
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    gender: 'Male',
    designation: '',
    departmentId: 1,
    salary: null,
    dateOfJoining: new Date().toISOString(),
    isActive: true
  };

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getEmployees();
  }

  getEmployees() {
    this.http.get<any[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.employees = data;
        this.filterEmployees();
      },
      error: (err) => console.error('API Error:', err)
    });
  }

  filterEmployees() {
    if (!this.searchTerm || this.searchTerm.trim() === '') {
      this.filteredEmployees = [...this.employees];
    } else {
      const term = this.searchTerm.toLowerCase().trim();
      this.filteredEmployees = this.employees.filter(emp =>
        (emp.firstName && emp.firstName.toLowerCase().includes(term)) ||
        (emp.lastName && emp.lastName.toLowerCase().includes(term)) ||
        (emp.employeeCode && emp.employeeCode.toLowerCase().includes(term)) ||
        (emp.designation && emp.designation.toLowerCase().includes(term))
      );
    }
  }

 saveEmployee() {
    if (this.isEditMode && this.editingId) {
      // Backend DbContext requirements
      const updateData = {
        employeeId: Number(this.editingId),
        id: Number(this.editingId),
        employeeCode: this.newEmp.employeeCode,
        firstName: this.newEmp.firstName,
        lastName: this.newEmp.lastName,
        email: this.newEmp.email,
        phone: this.newEmp.phone,
        gender: this.newEmp.gender || 'Male',
        designation: this.newEmp.designation,
        departmentId: Number(this.newEmp.departmentId || 1),
        salary: Number(this.newEmp.salary),
        dateOfJoining: this.newEmp.dateOfJoining || new Date().toISOString(),
        isActive: this.newEmp.isActive ?? true
      };

      this.http.put(`${this.apiUrl}/${this.editingId}`, updateData).subscribe({
        next: () => {
          alert('Employee Updated Successfully!');
          this.getEmployees();
          this.resetForm();
        },
        error: (err) => {
          console.error('Update Error:', err);
          alert(`Failed to update employee! Status: ${err.status}`);
        }
      });
    } else {
      this.http.post(this.apiUrl, this.newEmp).subscribe({
        next: () => {
          alert('Employee Added Successfully!');
          this.getEmployees();
          this.resetForm();
        },
        error: (err) => alert('Failed to add employee!')
      });
    }
  }

  // Edit Employee
  editEmployee(emp: any) {
    this.isEditMode = true;
    this.editingId = emp.employeeId ?? emp.id ?? emp.empId;
    this.newEmp = { 
      ...emp,
      departmentId: emp.departmentId || 1,
      gender: emp.gender || 'Male',
      dateOfJoining: emp.dateOfJoining || new Date().toISOString(),
      isActive: emp.isActive !== undefined ? emp.isActive : true
    };
  }

  // Delete Employee
  deleteEmployee(emp: any) {
    const targetId = typeof emp === 'object' ? (emp.employeeId || emp.id || emp.empId) : emp;
    if (!targetId) {
      alert('Invalid Employee ID!');
      return;
    }

    if (confirm('Are you sure you want to delete this employee?')) {
      this.http.delete(`${this.apiUrl}/${targetId}`).subscribe({
        next: () => {
          alert('Employee Deleted Successfully!');
          this.getEmployees();
        },
        error: (err) => alert('Failed to delete employee!')
      });
    }
  }

  resetForm() {
    this.isEditMode = false;
    this.editingId = null;
    this.newEmp = {
      employeeCode: '',
      firstName: '',
      lastName: '',
      email: '',
      phone: '',
      gender: 'Male',
      designation: '',
      departmentId: 1,
      salary: null,
      dateOfJoining: new Date().toISOString(),
      isActive: true
    };
  }
}