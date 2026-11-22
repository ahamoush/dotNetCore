# شرح IProjectRepository.cs

عقد التخزين. الطبقة Application تعرّفه. Infrastructure تنفّذه بـ Entity Framework.
هكذا لا يعرف منطق العمل اسم SQL Server.

- `ListAsync` كل الصفوف.
- `GetByIdAsync(id, tracking)` `tracking` يعني: هل ستعدّل الكيان بعد الجلب؟
- `AddAsync` أضف للسياق. الحفظ الفعلي في `SaveChangesAsync`.
- `AnyAsync` هل يوجد هذا الرقم؟
- `SearchAsync` يرجع زوجاً: القائمة، والعدد الكلي. القوسان `(List, int)` اسمهما tuple.
- `SaveChangesAsync` اكتب التغييرات المعلقة في SQL.
