# شرح AuthResult.cs

نتيجة داخلية من `AuthService` إلى `AuthController`. ليست جسم HTTP نفسه.

- `Succeeded` هل نجحت العملية.
- `Data` جواب التوكن إذا نجحت. `?` لأنه لا يوجد عند الفشل.
- `Errors` رسائل الفشل. المصفوفة تبدأ فارغة `[]`.

- `Success(data)` يصنع نتيجة ناجحة ويملأ `Data`.
- `Failure(params string[] errors)` يصنع نتيجة فاشلة. `params` يسمح بكتابة عدة نصوص: `Failure("a", "b")`.

الكونترولر يحوّل الفشل إلى 400 عند التسجيل و401 عند الدخول.
