# شرح CurrentUserDto.cs

جواب `GET /api/auth/me`. يُبنى من التوكن الحالي، بدون ضرب قاعدة البيانات.

- `Id` قيمة claim اسمها `sub`.
- `Email` من claim اسمها `email`.
- `FullName` من claim اسمها `name`.
- `Roles` الأدوار داخل التوكن.
- `Permissions` تُحسب في اللحظة من `RolePermissionMap` حتى لو لم تُخزَّن داخل التوكن.
