# شرح AuthService.cs

ينشئ المستخدم ويصدر JWT. لا يتحقق من شكل الحقول. الكونترولر فعل ذلك بـ FluentValidation.

## الحقول

- `_users` هو `UserManager<ApplicationUser>`.
- `_configuration` يقرأ `Jwt` من الإعدادات.

مسح خريطة أسماء الـ claims يحدث مرة في `Program.cs` قبل بناء التطبيق، حتى يبقى اسم `role` هو `role` ولا يتحول إلى اسم طويل خاص بمايكروسوفت.

## RegisterAsync

يبني مستخدماً: اسم الدخول والبريد هما البريد، و`FullName` من الطلب.
`CreateAsync(user, password)` يهشّر الكلمة ويفحص سياسة Identity.
إذا فشل، نجمع `Description` من كل خطأ في `AuthResult.Failure`.
إذا نجح، نضيف دور `Member`. فشل الدور أيضاً فشل للعملية.
النجاح يبني جواب التوكن.

## LoginAsync

`FindByEmailAsync`. إذا لا مستخدم، أو `CheckPasswordAsync` رفض الكلمة، نرجع فشلاً برسالة واحدة. لا نقول أيهما الخطأ.
النجاح يبني نفس جواب التوكن.

## CreateResponseAsync

1. أدوار المستخدم من Identity.
2. صلاحياته من `RolePermissionMap.ForRoles`.
3. وقت الانتهاء = الآن UTC زائد الدقائق من الإعدادات.
4. قائمة claims:
   - `sub` معرف المستخدم.
   - `email`.
   - `name` الاسم الظاهر.
   - claim `role` لكل دور، في حلقة واضحة.
5. مفتاح متماثل من نص `Jwt:Key` بترميز UTF-8.
6. توقيع `HmacSha256`.
7. `JwtSecurityToken` فيه المُصدر والجمهور والـ claims والانتهاء والتوقيع.
8. `WriteToken` يحوّل التوكن إلى نص من ثلاثة أجزاء تفصلها نقاط.

## ReadKey

يقرأ `Jwt:Key`. إذا فارغ أو أقصر من 32 حرفاً يتوقف. HMAC يحتاج مفتاحاً طويلاً.

## ReadExpiresInMinutes

يقرأ النص ويحاول تحويله إلى عدد. إذا نجح والعدد أكبر من صفر يرجعه. غير ذلك 60 دقيقة.
