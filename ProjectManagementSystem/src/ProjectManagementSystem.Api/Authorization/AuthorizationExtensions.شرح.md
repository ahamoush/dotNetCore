# شرح AuthorizationExtensions.cs

`AddPermissionPolicies` تُستدعى من `Program`.

أولاً `EnsureEveryPermissionHasARole`. إذا بقيت صلاحية بلا دور يتوقف التشغيل.

ثم `AddAuthorization`:

- `FallbackPolicy` سياسة احتياط. أي مسار بلا `[AllowAnonymous]` يطلب مستخدماً مسجّلاً.
  لذلك `GET /` بلا توكن يرجع 401 حتى لو لم نكتب `[Authorize]` على مسار فارغ.
  `register` و`login` عليهما `[AllowAnonymous]` فيفلتان من هذا.

- حلقة على `AppPermissions.All`.
  لكل صلاحية نجلب الأدوار التي تملكها بـ `RolesWith`.
  `AddPolicy` باسم الصلاحية. الشرط `RequireRole` أي يكفي دور واحد من القائمة.
  مثال: `Permissions.Projects.View` يسمح لـ Viewer وMember وTeamLeader وProjectManager وAdmin وSuperAdmin.

الكونترولر يكتب `[Authorize(Policy = AppPermissions.Projects.View)]`. الاسم يطابق مفتاح السياسة.
