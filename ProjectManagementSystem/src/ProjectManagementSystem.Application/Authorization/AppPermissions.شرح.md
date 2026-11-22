# شرح AppPermissions.cs

أسماء الصلاحيات. هي نصوص سياسات `[Authorize(Policy = ...)]` وليست أدواراً.

- الكلاس الداخلي `Projects` يجمع صلاحيات المشاريع: View وCreate وUpdate وDelete.
- `Tasks` نفس الأربع للمهام.
- `Users` فيها `View` و`ManageRoles`.

كل نص يبدأ بـ `Permissions.` حتى لا يختلط مع اسم دور.

- `All` مصفوفة بكل الصلاحيات. `Admin` و`SuperAdmin` يأخذانها كاملة.
  وتُستخدم أيضاً لنتأكد عند التشغيل أن كل صلاحية مربوطة بدور واحد على الأقل.
