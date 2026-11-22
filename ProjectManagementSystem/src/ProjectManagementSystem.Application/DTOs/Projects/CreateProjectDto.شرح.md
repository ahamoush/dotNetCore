# شرح CreateProjectDto.cs

البيانات التي يرسلها العميل عند إنشاء مشروع. لا يوجد هنا `Id` ولا `Status` لأن الخادم يقررهما.

- `namespace ...DTOs.Projects`
  صندوق كائنات النقل الخاصة بالمشاريع.

- `public class CreateProjectDto`
  كيس خصائص فقط. لا منطق داخله.

- `public string Name { get; set; } = string.Empty;`
  الاسم القادم من JSON. التحقق من الطول موجود في `CreateProjectDtoValidator` وليس هنا.

- `public string Description { get; set; } = string.Empty;`
  الوصف. مسموح أن يبقى فارغاً، والتحقق يمنع فقط أن يزيد عن 500 حرفاً.
