# شرح ITaskService.cs

عقد خدمة المهام.

- `GetAllAsync(int? projectId)` الكل، أو مهام مشروع واحد.
- `GetByIdAsync` واحدة أو `null`.
- `CreateAsync` ترجع `null` إذا كان `ProjectId` لا يطابق مشروعاً.
- `UpdateAsync` و`DeleteAsync` ترجعان `false` إذا لم توجد المهمة.
