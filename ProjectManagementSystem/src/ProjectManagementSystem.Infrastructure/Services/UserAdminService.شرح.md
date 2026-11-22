# شرح UserAdminService.cs

قائمة المستخدمين وتغيير أدوارهم.

## GetUsersAsync

يجلب المستخدمين مرتبين بالبريد، بلا تتبع.
ثم لكل مستخدم يستدعي `GetRolesAsync`. هذا سؤال إضافي لكل مستخدم.
القائمة صغيرة لأنها شاشة إدارة، والحلقة أسهل من `Join`.
لكل مستخدم نبني `UserResponseDto` ونجمع الأدوار بلا تكرار ثم `Sort`.

## GetRoles

لا SQL. يدور على `AppRoles.All` ويبني `RoleResponseDto` من الخريطة في الذاكرة.

## SetRolesAsync

الخطوات بالترتيب، وأول فشل يرجع فوراً:

1. رقم المستخدم فارغ: Invalid.
2. الفاعل يغيّر نفسه: Forbidden. لا أحد يعدّل أدواره من هذه الشاشة.
3. حلقة تنظّف الأسماء: `Trim`، وتتجاهل الفارغ، وتمنع التكرار.
4. لا دور بعد التنظيف: Invalid.
5. حلقة تجمع الأسماء غير الموجودة في `AppRoles.All`.
6. المستخدم غير موجود: NotFound.
7. إذا كان الطلب أو الوضع الحالي يلمس `SuperAdmin` والفاعل ليس SuperAdmin: Forbidden.
8. إذا كنا سنزيل SuperAdmin وهو آخر واحد: Forbidden. `GetUsersInRoleAsync` يعدّهم.
9. حلقة `toRemove`: ما هو حالي وغير مطلوب.
10. حلقة `toAdd`: ما هو مطلوب وغير حالي.
11. لا فرق: Ok بدون كتابة.
12. `BeginTransactionAsync` حتى الإزالة والإضافة تنجحان معاً أو لا تُحفظ أي منهما.
13. `RemoveFromRolesAsync` ثم `AddToRolesAsync`. الفشل يرجع Invalid. التخلص من المعاملة غير المثبتة يلغي الكتابة.
14. `CommitAsync` ثم Ok.

`JoinErrors` حلقة تجمع وصف أخطاء Identity في نص واحد.
