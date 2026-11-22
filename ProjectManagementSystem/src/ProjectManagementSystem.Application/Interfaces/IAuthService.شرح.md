# شرح IAuthService.cs

عقد التسجيل والدخول.

- `RegisterAsync(RegisterDto)` ينشئ مستخدماً بدور `Member` ويرجع `AuthResult`.
- `LoginAsync(LoginDto)` يتحقق من البريد وكلمة السر ويرجع توكناً أو فشلاً.

الواجهة في Application. التنفيذ `AuthService` في Infrastructure لأنه يحتاج Identity وJWT.
