using Microsoft.AspNetCore.Mvc;

// هذا السطر يستورد مكتبة ASP.NET Core MVC.

// نحتاجها حتى نستطيع استخدام أشياء مثل:
// - ControllerBase
// - [ApiController]
// - [Route]
// - [HttpGet]
// - [HttpPost]
// - IActionResult
// - Ok()
// - NotFound()
// - BadRequest()
// بدونها لن يتعرف الكود على هذه العناصر.

using ProjectManagementSystem.DTOs.Projects;
// هذا يستورد DTOs الخاصة بالمشاريع.
using ProjectManagementSystem.Interfaces;
//يستورد الـ Interfaces الموجودة في المشروع.
namespace ProjectManagementSystem.Controllers;
// هذا يحدد المكان المنطقي الذي ينتمي إليه الكلاس.

// يعني هذا الكلاس موجود ضمن مجلد أو مساحة أسماء اسمها:

// ProjectManagementSystem.Controllers

// الـ Namespace يساعدنا على تنظيم الكود وتجنب تعارض أسماء الكلاسات.


[ApiController]

// هذا Attribute يخبر ASP.NET Core أن هذا الكلاس يمثل API Controller.

// ويعطينا ميزات، منها:
// - التحقق التلقائي من صحة البيانات عند استخدام Model Validation.
// - إرجاع 400 Bad Request تلقائياً عند فشل التحقق من ModelState.
// - استنتاج مصدر بعض المعاملات مثل [FromBody] و[FromQuery].

[Route("api/[controller]")]
// هذا يحدد المسار الأساسي للـ API.
// يعني أن جميع الطلبات التي تبدأ بـ /api/projects ستصل إلى هذا الكلاس.

public class ProjectsController : ControllerBase
{
    // - : ControllerBase: يرث من ControllerBase حتى نستخدم وظائف الـ API مثل Ok() وNotFound().
    private readonly IProjectService _service;

    // IProjectService : هي الـ Interface التي تحدد العمليات المتعلقة بالمشاريع.
    //  فائدتها : تحدد العمليات المتعلقة بالمشاريع وتحدد العمليات المتعلقة بالمشاريع.

    public ProjectsController(IProjectService service)
    {
        _service = service;
        // ربط الـ Service مع الـ Controller بواسطة الـ Dependency Injection.
        // هذا يعني أنه عندما يتم إنشاء كائن من الـ ProjectsController، سيتم إنشاء كائن من الـ IProjectService وإربطه بالـ _service.
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetAll()
    {
        // Task
        // عملية غير متزامنة
        // تعني أن نتيجة الدالة ستصل لاحقاً، وليس بالضرورة فور استدعائها.

        // ActionResult
        // نتيجة HTTP
        // تسمح للدالة بإرجاع بيانات أو HTTP Status Codes مثل 200 و404 و400.


        // IEnumerable
        // مجموعة من العناصر
        // تعني أننا نريد مجموعة مشاريع، وليس مشروعاً واحداً.



        var projects = await _service.GetAllAsync();

        return Ok(projects);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ProjectResponseDto>>> GetProjects(
        [FromQuery] ProjectQueryDto query)
    {
        if (query.PageNumber < 1 ||
            query.PageSize < 1 ||
            query.PageSize > 100)
        {
            return BadRequest(new { message = "Invalid pagination parameters." });
        }

        var result = await _service.GetProjectsAsync(query);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById(int id)
    {
        var project = await _service.GetByIdAsync(id);

        if (project is null)
            return NotFound(new { message = "Project not found." });

        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> Create(CreateProjectDto dto)
    {
        var project = await _service.CreateAsync(dto);
        return Ok(project);

        // return CreatedAtAction(
        //     nameof(GetById),
        //     new { id = project.Id },
        //     project
        // );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProjectDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound(new { message = "Project not found." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Project not found." });

        return NoContent();
    }
}
