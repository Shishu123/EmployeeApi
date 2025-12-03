using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using EmployeeApi.Data;
using EmployeeApi.Models;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public EmployeeController(AppDbContext context) => _context = context;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEmployees() => Ok(await _context.Employees.ToListAsync());

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            return emp == null ? NotFound() : Ok(emp);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee(Employee emp)
        {
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployee), new { id = emp.EmpId }, emp);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee emp)
        {
            if (id != emp.EmpId) return BadRequest();
            _context.Entry(emp).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return NotFound();
            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
