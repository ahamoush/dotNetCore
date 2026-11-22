# شرح ITaskRepository.cs

عقد تخزين المهام.

- `ListAsync(int? projectId)` مع تصفية اختيارية.
- `GetByIdAsync` مع خيار التتبع.
- `AddAsync` إضافة للسياق.
- `Remove` حذف من السياق. ليست `Task` لأنها لا تنتظر الشبكة. الحذف الحقيقي عند `SaveChangesAsync`.
- `SaveChangesAsync` حفظ.
