using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Validation;
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
    private readonly IValidator<CreateTaskDto> _createValidator;
    private readonly IValidator<UpdateTaskDto> _updateValidator;

    public TasksController(
        ITaskService service,
        IValidator<CreateTaskDto> createValidator,
        IValidator<UpdateTaskDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [Authorize(Policy = AppPermissions.Tasks.View)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll([FromQuery] int? projectId)
    {
        IEnumerable<TaskResponseDto> tasks = await _service.GetAllAsync(projectId);
        return Ok(tasks);
    }

    [Authorize(Policy = AppPermissions.Tasks.View)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(int id)
    {
        TaskResponseDto? task = await _service.GetByIdAsync(id);
        if (task is null)
            return NotFound(new { message = "Task not found." });

        return Ok(task);
    }

    [Authorize(Policy = AppPermissions.Tasks.Create)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskDto dto)
    {
        IActionResult? invalid = await InputValidation.CheckAsync(
            _createValidator,
            dto,
            HttpContext.RequestAborted);
        if (invalid is not null)
            return invalid;

        TaskResponseDto? task = await _service.CreateAsync(dto);
        if (task is null)
            return BadRequest(new { message = "Project does not exist." });

        return Ok(task);
    }

    [Authorize(Policy = AppPermissions.Tasks.Update)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
    {
        IActionResult? invalid = await InputValidation.CheckAsync(
            _updateValidator,
            dto,
            HttpContext.RequestAborted);
        if (invalid is not null)
            return invalid;

        bool updated = await _service.UpdateAsync(id, dto);
        if (!updated)
            return NotFound(new { message = "Task not found." });

        return NoContent();
    }

    [Authorize(Policy = AppPermissions.Tasks.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Task not found." });

        return NoContent();
    }
}
