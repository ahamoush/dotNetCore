# شرح ApplicationDbContextFactory.cs

هذا الملف ليس لمسار الطلب. أداة `dotnet ef` تحتاجه وقت تصميم الترحيلات، حين لا يكون `Program.cs` شغّالاً.

`IDesignTimeDbContextFactory<ApplicationDbContext>` عقد الأداة.

`CreateDbContext`:
1. يبني `ConfigurationBuilder`.
2. `SetBasePath` مجلد التشغيل الحالي. عند أمر الترحيل يجب أن يكون مجلد الـ Api حتى يجد `appsettings.json`.
3. يقرأ `appsettings.json` إجبارياً، و`appsettings.Development.json` إن وُجد.
4. يأخذ `ConnectionStrings:DefaultConnection`. إذا غاب يرمي استثناء.
5. يبني خيارات SQL Server ويرجع سياقاً جديداً.

لا يسجل خدمات ولا يشغّل الموقع.
