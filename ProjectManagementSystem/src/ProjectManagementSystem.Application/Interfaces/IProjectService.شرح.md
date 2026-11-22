# شرح IProjectService.cs

عقد خدمة المشاريع. الكونترولر يعتمد على هذه الواجهة لا على الكلاس.

كل دالة `Task<...>` لأنها غير متزامنة: تنتظر قاعدة البيانات دون تجميد الخيط.

- `GetAllAsync` كل المشاريع بلا صفحات.
- `GetByIdAsync` واحد أو `null`.
- `CreateAsync` يرجع DTO المشروع الجديد.
- `UpdateAsync` و`DeleteAsync` ترجعا `bool`. `false` يعني غير موجود.
- `GetProjectsAsync` صفحة بحث.
