using CompanyApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly CompanyDbContext _context;

        public DepartmentController(CompanyDbContext context)
        {
            _context = context;
        }

        // Other action methods

        [HttpGet]
        public IActionResult GetDepartments()
        {
            var departments = _context.Departments.ToList();
            var departmentDTOs = departments.Select(ConvertToDTO);
            return Ok(departmentDTOs);
        }

        [HttpPost]
        public IActionResult CreateDepartment([FromBody] DepartmentDTO departmentDTO)
        {
            if (departmentDTO == null)
            {
                return BadRequest();
            }

            var department = ConvertToEntity(departmentDTO);

            _context.Departments.Add(department);
            _context.SaveChanges();

            var createdDepartmentDTO = ConvertToDTO(department);

            return CreatedAtRoute("GetDepartment", new { id = createdDepartmentDTO.Id }, createdDepartmentDTO);
        }

        // Other action methods

        private static DepartmentDTO ConvertToDTO(Department department)
        {
            return new DepartmentDTO
            {
                Id = department.DepartmentId,
                DepartmentName = department.Name
            };
        }

        private static Department ConvertToEntity(DepartmentDTO departmentDTO)
        {
            return new Department
            {
                DepartmentId = departmentDTO.Id,
                Name = departmentDTO.DepartmentName
            };
        }
    }
}