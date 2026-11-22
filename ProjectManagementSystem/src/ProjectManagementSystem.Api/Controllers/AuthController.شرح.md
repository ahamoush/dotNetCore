# شرح AuthController.cs

المسار `api/auth`. لا `[Authorize]` على الكلاس حتى نفتح التسجيل والدخول.

- `POST register` عليه `[AllowAnonymous]` صراحة، بسبب السياسة الاحتياط التي تطلب توكناً لكل شيء آخر.
  يتحقق من `RegisterDto`. إذا فشل Identity (بريد مكرر مثلاً) نرجع 400 و`errors`.
  النجاح 200 وجسم `AuthResponseDto`.
- `POST login` نفس السماح. فشل التحقق 400. فشل البريد أو الكلمة 401 برسالة واحدة. النجاح 200 والتوكن.
- `GET me` يحتاج `[Authorize]`. يبني `CurrentUserDto` من التوكن:
  حلقة على claims الدور، و`Contains` تمنع التكرار.
  `FindFirst("sub")` المعرف. `??` إذا غاب نضع نصاً فارغاً.
  الصلاحيات تُحسب من الأدوار عبر الخريطة.
