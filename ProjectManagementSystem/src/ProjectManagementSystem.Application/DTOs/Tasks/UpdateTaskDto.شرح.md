# شرح UpdateTaskDto.cs

جسم تعديل مهمة. لا يغيّر المشروع المالك. `ProjectId` غير موجود هنا عن قصد.

- `Title` و `Description` نفس حدود الإنشاء.
- `IsCompleted` نعم أو لا. ليست اختيارية. إذا غابت من JSON تصير `false`.
- `DueDate` اختياري.
