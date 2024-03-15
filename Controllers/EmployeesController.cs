using CompanyApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly CompanyDbContext _context;
        public EmployeesController(CompanyDbContext context)
        {
            _context = context;
        }
        private static EmployeeDTO EmployeeToDTO(Employee employee)
        {
            return new EmployeeDTO
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                ManagerId = employee.ManagerId,
                Salary = employee.Salary,
                Bonus = employee.Bonus,
                DepartmentId = employee.DepartmentId
            };
        }

        private static Employee DTOToEmployee(EmployeeDTO employeeDTO)
        {
            return new Employee
            {
                EmployeeId = employeeDTO.EmployeeId,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                ManagerId = employeeDTO.ManagerId,
                Salary = employeeDTO.Salary,
                Bonus = employeeDTO.Bonus,
                DepartmentId = employeeDTO.DepartmentId
            };
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee()
        {
            var employees = await _context.Employees
                .Select(e => EmployeeToDTO(e))
                .ToListAsync();

            return employees;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            var employeeDTO = EmployeeToDTO(employee);
            return employeeDTO;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.EmployeeId)
            {
                return BadRequest();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee.EmployeeId = employeeDTO.EmployeeId;
            employee.FirstName = employeeDTO.FirstName;
            employee.LastName = employeeDTO.LastName;
            employee.ManagerId = employeeDTO.ManagerId;
            employee.Salary = employeeDTO.Salary;
            employee.Bonus = employeeDTO.Bonus;
            employee.DepartmentId = employeeDTO.DepartmentId;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> PostEmployee(EmployeeDTO employeeDTO)
        {
            var employee = DTOToEmployee(employeeDTO);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, EmployeeToDTO(employee));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeDTO>> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return EmployeeToDTO(employee);
        }
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}