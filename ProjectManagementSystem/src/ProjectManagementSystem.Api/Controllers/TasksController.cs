using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Application.Authorization;
using ProjectManagementSystem.Application.DTOs.Tasks;
using ProjectManagementSystem.Application.Interfaces;

namespace ProjectManagementSystem.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;

    public TasksController(ITaskService service)
    {
        _service = service;
    }

    [Authorize(Policy = AppPermissions.Tasks.View)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll(
        [FromQuery] int? projectId)
    {
        var tasks = await _service.GetAllAsync(projectId);
        return Ok(tasks);
    }

    [Authorize(Policy = AppPermissions.Tasks.View)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(int id)
    {
        var task = await _service.GetByIdAsync(id);
        if (task is null)
            return NotFound(new { message = "Task not found." });

        return Ok(task);
    }

    [Authorize(Policy = AppPermissions.Tasks.Create)]
    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskDto dto)
    {
        var task = await _service.CreateAsync(dto);
        if (task is null)
            return BadRequest(new { message = "Project does not exist." });

        return Ok(task);
    }

    [Authorize(Policy = AppPermissions.Tasks.Update)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (!updated)
            return NotFound(new { message = "Task not found." });

        return NoContent();
    }

    [Authorize(Policy = AppPermissions.Tasks.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Task not found." });

        return NoContent();
    }
}
