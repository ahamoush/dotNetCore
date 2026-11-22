# شرح ApplicationUser.cs

مستخدم Identity مع حقل زائد.

- `IdentityUser` كلاس جاهز فيه `Id` و`Email` و`UserName` وهاش كلمة السر وخانات القفل.
- `FullName` الحقل الذي أضفناه. طوله 100 وإلزامي، وهذا مضبوط في `OnModelCreating` داخل السياق.

لا تضع كلمة السر هنا كنص. Identity يخزن هاشاً في عمود جاهز.
