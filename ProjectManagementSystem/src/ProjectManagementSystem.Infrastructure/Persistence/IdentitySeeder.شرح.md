# شرح IdentitySeeder.cs

يزرع البيانات الأولى مرة واحدة عند الإقلاع. `Program` يستدعي `SeedAsync` داخل scope.

## SeedAsync

أولاً الأدوار، ثم مستخدم SuperAdmin.

## SeedRolesAsync

يجلب `RoleManager<IdentityRole>`.
يدور على `AppRoles.All`.
إذا الدور موجود يتخطاه بـ `continue`.
وإلا ينشئه. إذا فشل Identity يجمع أوصاف الأخطاء ويرمي استثناء يوقف التشغيل. لا نكمل بنظام بلا أدوار.

## SeedSuperAdminAsync

يقرأ من الإعدادات:
- `Seed:SuperAdmin:Email`
- `Seed:SuperAdmin:Password`
- `Seed:SuperAdmin:FullName`

إذا غاب البريد أو كلمة السر يخرج بهدوء. هذا يسمح بتشغيل بلا زرع.

يبحث عن المستخدم بالبريد.
إذا لم يوجد ينشئ `ApplicationUser` بنفس البريد كاسم مستخدم، و`EmailConfirmed = true` حتى لا يعلق على تأكيد البريد.
إذا فشل الإنشاء يتوقف.

ثم إذا لم يكن في دور `SuperAdmin` يضيفه. الفشل يوقف التشغيل.

التشغيل التالي يجد المستخدم والدور فلا يكرر شيئاً.
