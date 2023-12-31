using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Services.EmployeeService;

namespace SOAProject.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
    }

    [Authorize(Policy = "role")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployee()
    {
        var items = await _employeeService.GetAllEmployee();
        return Ok(items);
    }

    [Authorize(Policy = "role")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployeeById(int id)
    {
        var item = await _employeeService.GetEmployeeById(id);
        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }
    
    [Authorize(Policy = "role")]
    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee(EmployeeDTO item)
    {
        Employee employee = new Employee()
        {
            Name = item.Name,
            SurName = item.SurName,
            Tel = item.Tel,
            DepartmentId = item.DepartmentId
        };
        await _employeeService.CreateEmployee(employee);
        return Ok(item);
    }


    [Authorize(Policy = "role")]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateEmployeeAsync(int id, EmployeeDTO employee)
    {
        if (id != employee.Id)
        {
            return BadRequest();
        }

        try
        {
            var message = await _employeeService.UpdateEmployeeAsync(id, employee);
            return Ok(message);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [Authorize(Policy = "role")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        var item = await _employeeService.GetEmployeeById(id);
        if (item == null)
        {
            return NotFound();
        }

        var message = await _employeeService.DeleteEmployee(item.Id);
        return Ok(message);
    }
}