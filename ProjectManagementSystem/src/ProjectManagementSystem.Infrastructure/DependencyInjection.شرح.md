# شرح DependencyInjection.cs في Infrastructure

`AddInfrastructure` يربط قاعدة البيانات وIdentity والمستودعات. `Program` يمرر `configuration` حتى نقرأ نص الاتصال.

## قاعدة البيانات

`AddDbContext<ApplicationDbContext>` يسجل السياق كـ Scoped: طلب واحد، سياق واحد.
`UseSqlServer(configuration.GetConnectionString("DefaultConnection"))` يقرأ المفتاح من `appsettings.json`.

## Identity

`AddIdentityCore<ApplicationUser>` مستخدمون بدون واجهة صفحات Identity.
قواعد كلمة السر:
- البريد فريد.
- 8 أحرف على الأقل.
- رقم، وحرف كبير، وحرف صغير.
- الرمز الخاص غير مطلوب (`RequireNonAlphanumeric = false`).

ثم:
- `AddRoles<IdentityRole>()` أدوار.
- `AddEntityFrameworkStores` الجداول في نفس السياق.

التطبيق يتحقق من كلمة السر عبر `UserManager` ويصدر JWT. لا يوجد تسجيل دخول بالكوكيز ولا رموز تأكيد بريد أو استعادة كلمة سر، لذلك لا يُسجَّل `SignInManager` ولا مزوّدات الرموز.

## الخدمات

- المستودعان Scoped مثل السياق.
- `IAuthService` و`IUserAdminService` تنفيذات هذه الطبقة.
- `IClaimsTransformation` تُستدعى تلقائياً بعد نجاح JWT، قبل فحص الصلاحية. التنفيذ `PermissionClaimsTransformation`.
