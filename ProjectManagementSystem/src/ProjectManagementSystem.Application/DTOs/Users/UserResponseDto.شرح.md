# شرح UserResponseDto.cs

صف واحد في قائمة المستخدمين.

- `Id` معرف Identity، وهو نص وليس رقماً.
- `Email` البريد.
- `FullName` الاسم.
- `Roles` الأدوار الحالية. `IList<string>` قائمة نصوص تبدأ فارغة.
