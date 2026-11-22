# شرح CreateProjectDtoValidator.cs

قواعد رفض إنشاء مشروع. FluentValidation ينفّذها قبل ما تصل الخدمة.

- `AbstractValidator<CreateProjectDto>`
  هذا الـ Validator مسؤول عن هذا النوع فقط.

- `RuleFor(x => x.Name)`
  ابدأ قواعد الاسم. `x => x.Name` يعني "انظر إلى الخاصية Name".

- `.NotEmpty()` لا نص فارغ ولا مسافات فقط.
- `.MinimumLength(3)` ثلاثة أحرف على الأقل.
- `.MaximumLength(100)` مئة على الأكثر.

- `RuleFor(x => x.Description).MaximumLength(500)`
  الوصف اختياري، لكن إن وُجد لا يتجاوز 500.
