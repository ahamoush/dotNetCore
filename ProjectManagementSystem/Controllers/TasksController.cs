using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.DTOs.Tasks;
using ProjectManagementSystem.Interfaces;

namespace ProjectManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;

    public TasksController(ITaskService service)
    {
        _service = service;
    }

    // GET /api/tasks
    // أو: GET /api/tasks?projectId=1
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll(
        [FromQuery] int? projectId)
    {
        var tasks = await _service.GetAllAsync(projectId);
        return Ok(tasks);
    }

    // GET /api/tasks/3
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(int id)
    {
        var task = await _service.GetByIdAsync(id);

        if (task is null)
            return NotFound(new { message = "Task not found." });

        return Ok(task);
    }

    // POST /api/tasks
    // Body مثال:
    // { "title": "Setup DB", "description": "Create tables", "projectId": 1 }
    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskDto dto)
    {
        var task = await _service.CreateAsync(dto);

        // لو المشروع مش موجود نرجع 400 Bad Request
        if (task is null)
            return BadRequest(new { message = "Project does not exist." });

        return CreatedAtAction(
            nameof(GetById),
            new { id = task.Id },
            task
        );
    }

    // PUT /api/tasks/3
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound(new { message = "Task not found." });

        return NoContent();
    }

    // DELETE /api/tasks/3  (Hard Delete)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Task not found." });

        return NoContent();
    }
}
