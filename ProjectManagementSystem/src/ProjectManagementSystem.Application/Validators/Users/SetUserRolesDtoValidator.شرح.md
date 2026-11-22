# شرح SetUserRolesDtoValidator.cs

- `.Cascade(CascadeMode.Stop)`
  إذا فشلت قاعدة، لا تكمل القواعد التي بعدها على نفس القائمة. لو كانت `Roles` فارغة لا نفحص التكرار.

- `.NotNull()` القائمة نفسها ليست `null`.
- `.NotEmpty()` فيها عنصر واحد على الأقل.
- `.Must(...)` قاعدة نكتبها بأنفسنا.
  `Distinct` يحذف المكرر. إذا بقي العدد كما هو فلا تكرار.
  المقارنة `OrdinalIgnoreCase` تعني `Admin` و`admin` يُحسبان تكراراً.
- `.WithMessage` رسالة التكرار.

- `RuleForEach(x => x.Roles)`
  افحص كل عنصر وحده.

- `.NotEmpty()` العنصر ليس نصاً فارغاً.
- `.Must(role => AppRoles.All.Contains(role, StringComparer.Ordinal))`
  الاسم موجود حرفياً في قائمة الأدوار. `Admin` مقبول. `admin` مرفوض.
- `.When(x => x.Roles is not null)`
  لا تفحص العناصر إذا كانت القائمة `null`. قاعدة `NotNull` أعلاه تتولى ذلك.
