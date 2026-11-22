# شرح UsersController.cs

المسار `api/users`. الكلاس يطلب توكناً.

- `GET` صلاحية `Permissions.Users.View`. يرجع قائمة المستخدمين.
- `PUT api/users/{id}/roles` صلاحية `ManageRoles`.
  1. يتحقق من `SetUserRolesDto`.
  2. يقرأ `sub` من التوكن. إذا غاب: 401.
  3. يستدعي الخدمة. المعامل الأخير `User.IsInRole(SuperAdmin)` يقرأ الأدوار بعد تحديثها من قاعدة البيانات.
  4. `Ok` يصير 204.
  5. `NotFound` يصير 404.
  6. `Forbidden` يصير 403.
  7. غير ذلك 400.

`if` المتتالية أسهل من `switch` للمبتدئ. المعنى واحد.
