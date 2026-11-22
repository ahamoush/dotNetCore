# شرح Project.cs

كيان المشروع كما يُخزَّن في قاعدة البيانات. ليس هذا ما يراه متصفح الـ API. الـ API يرى `ProjectResponseDto`.

## الأسطر

- `using ProjectManagementSystem.Domain.Enums;`
  جلب صندوق `ProjectStatus` لأن الخاصية `Status` من ذلك النوع.

- `public class Project : BaseEntity`
  مشروع. النقطتان تعني أنه يرث `Id` و`CreatedAt` و`UpdatedAt` و`IsDeleted` و`DeletedAt`.

- `public string Name { get; set; } = string.Empty;`
  اسم المشروع. نص. البداية نص فارغ حتى لا تكون `null`.

- `public string Description { get; set; } = string.Empty;`
  الوصف. نفس الفكرة.

- `public ProjectStatus Status { get; set; } = ProjectStatus.New;`
  الحالة. البداية `New` أي مشروع جديد.

- `public DateTime StartDate { get; set; }`
  تاريخ البداية. ليس له قيمة أولى في الكود، فتكون `0001-01-01` إلى أن تُملأ.

- `public DateTime? DueDate { get; set; }`
  تاريخ الاستحقاق. اختياري بسبب `?`.

- `public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();`
  المهام التابعة لهذا المشروع.
  `ICollection` يعني مجموعة يمكن الإضافة إليها.
  `= new List<...>()` قائمة فارغة جاهزة حتى لا تنهار الإضافة على `null`.
