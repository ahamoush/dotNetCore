using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.DTOs.Projects;
using ProjectManagementSystem.Interfaces;

namespace ProjectManagementSystem.Controllers;

// ApiController: يفعّل ميزات الـ API مثل Validation التلقائي
// ميزات ApiController :
// 1. Validation التلقائي مثال: [FromQuery] ProjectQueryDto query   
// 2. Model Binding التلقائي مثال: [FromBody] CreateProjectDto dto
// 3. Action Results التلقائي مثال: return Ok(projects);
// 4. Formatting التلقائي مثال: return Json(projects);
// 5. CORS التلقائي مثال: return Cors();
// 6. Security التلقائي مثال: return Authorize();
// 7. Logging التلقائي مثال: return Log();
// 8. Error Handling التلقائي مثال: return Error();   
[ApiController]
// المسار الأساسي: /api/projects
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    // ControllerBase يحتوي على ميزات الـ API مثل Validation التلقائي
    // مثال: return BadRequest(new { message = "Invalid request." });
    // مثال: return NotFound(new { message = "Resource not found." });
    // مثال: return Ok(project);
    // مثال: return NoContent();
    // مثال: return Conflict(new { message = "Resource already exists." });
    // مثال: return Unauthorized(new { message = "Unauthorized." });
    // مثال: return Forbidden(new { message = "Forbidden." });
    // مثال: return BadRequest(new { message = "Invalid request." });
    private readonly IProjectService _service;

    // Dependency Injection: ASP.NET يمرر IProjectService تلقائياً
    public ProjectsController(IProjectService service)
    {
        _service = service;
    }

    // GET /api/projects/all
    // يرجع كل المشاريع بدون Pagination
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetAll()
    {
        var projects = await _service.GetAllAsync();
        return Ok(projects); // 200 OK
    }

    // GET /api/projects?search=web&status=1&pageNumber=1&pageSize=10&sortBy=name&descending=false
    // بحث + فلترة + ترتيب + صفحات
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ProjectResponseDto>>> GetProjects(
        [FromQuery] ProjectQueryDto query)
    {
        // تحقق بسيط من معاملات الصفحات
        if (query.PageNumber < 1 ||
            query.PageSize < 1 ||
            query.PageSize > 100)
        {
            return BadRequest(new { message = "Invalid pagination parameters." });
        }

        var result = await _service.GetProjectsAsync(query);
        return Ok(result);
    }

    // GET /api/projects/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById(int id)
    {
        var project = await _service.GetByIdAsync(id);

        if (project is null)
            return NotFound(new { message = "Project not found." }); // 404

        return Ok(project);
    }

    // POST /api/projects
    // Body مثال:
    // { "name": "My Project", "description": "Learning ASP.NET Core" }
    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> Create(CreateProjectDto dto)
    {
        var project = await _service.CreateAsync(dto);

        return Ok(project);
    }

    // PUT /api/projects/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProjectDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound(new { message = "Project not found." });

        return NoContent(); // 204: نجح التعديل بدون Body
    }

    // DELETE /api/projects/5
    // ملاحظة: هذا Soft Delete (المشروع يختفي من النتائج لكنه يفضل في الداتابيز)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Project not found." });

        return NoContent();
    }
}
