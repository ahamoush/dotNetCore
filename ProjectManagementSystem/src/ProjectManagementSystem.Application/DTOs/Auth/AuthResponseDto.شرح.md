# شرح AuthResponseDto.cs

الجواب بعد تسجيل ناجح أو دخول ناجح.

- `Token` نص JWT. العميل يرسله لاحقاً في الترويسة `Authorization: Bearer ...`.
- `ExpiresAt` متى ينتهي التوكن.
- `Email` و `FullName` للعرض.
- `Roles` أسماء الأدوار مثل `Member`.
- `Permissions` أسماء الصلاحيات المحسوبة من الأدوار، مثل `Permissions.Projects.View`.
