# شرح AppClaimTypes.cs

أسماء الخانات داخل التوكن. الخانة اسم ونص.

- `Role = "role"`
  خانة الدور. JWT يضع دوراً واحداً في كل claim، وإذا تعددت الأدوار تتكرر الخانة.

- `Name = "name"`
  الاسم الظاهر. ليس اسم المستخدم الخاص بـ Identity.

- `PermissionsSynced = "permissions_synced"`
  علم داخلي. قيمته `"true"` بعد ما نعيد قراءة الأدوار من قاعدة البيانات في هذا الطلب.
  يمنع `PermissionClaimsTransformation` من سؤال SQL مرة ثانية لنفس الطلب.
