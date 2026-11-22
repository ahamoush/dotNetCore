# شرح RegisterDtoValidator.cs

- `FullName`: غير فارغ، من 3 إلى 100.
- `Email`: غير فارغ، `.EmailAddress()` يفحص شكل البريد، والطول حتى 256.
- `Password`:
  - غير فارغة.
  - من 8 إلى 100.
  - `.Matches("[A-Z]")` فيها حرف إنجليزي كبير. القوس يعني "أي حرف من A إلى Z".
  - `.Matches("[a-z]")` حرف صغير.
  - `.Matches("[0-9]")` رقم.
  - `.WithMessage("...")` يستبدل الرسالة الإنجليزية الافتراضية بهذه الجملة.

Identity في `DependencyInjection` يطلب نفس الشروط. هنا نرجع 400 موضحاً الحقل قبل أن نصل إلى Identity.
