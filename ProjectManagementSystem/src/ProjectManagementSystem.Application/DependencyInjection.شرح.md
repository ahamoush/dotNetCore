# شرح DependencyInjection.cs في Application

دالة واحدة تسجل خدمات هذه الطبقة. `Program.cs` يستدعي `AddApplication()`.

- `AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly)`
  ابحث في مشروع Application عن كل كلاس Validator وسجّله في حقن التبعيات.
  `Assembly` هنا يعني "ملف dll هذا المشروع". لذلك لا نسجل كل Validator بخط وحده.

- `AddAutoMapper(_ => { }, typeof(DependencyInjection))`
  سجل AutoMapper وامسح المشروع بحثاً عن `Profile`.
  `_ => { }` دالة فارغة. المكتبة تطلبها لأن الإصدار 15 وما بعده يضع مفتاح الترخيص داخلها.
  لا نضع مفتاحاً في الكود. التطوير يعمل مع تحذير. الإنتاج يقرأ المتغير `AUTOMAPPER_LICENSE_KEY`.

- `AddScoped<IProjectService, ProjectService>()`
  كل طلب HTTP يحصل على `ProjectService` واحد، ويُرمى بعده.
  الواجهة `IProjectService` هي ما يطلبه الكونترولر. التنفيذ `ProjectService` هو الكلاس الحقيقي.

- `AddScoped<ITaskService, TaskService>()`
  نفس الفكرة للمهام.

- `AssertMappingsAreValid`
  بعد بناء التطبيق، `Program` يستدعيها.
  `GetRequiredService<IConfigurationProvider>()` يجلب خريطة AutoMapper.
  `AssertConfigurationIsValid()` يرمي خطأ فوراً إذا نسي الربط حقلاً مطلوباً.
  أفضل أن يتوقف التشغيل من أن يكتشف العميل الخطأ لاحقاً.
