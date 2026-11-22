# شرح UpdateProjectDtoValidator.cs

نفس قواعد الإنشاء، على كائن التعديل.

- الاسم: غير فارغ، من 3 إلى 100.
- الوصف: 500 كحد أقصى.

فصلناه عن Validator الإنشاء لأن النوع مختلف: `UpdateProjectDto` ليس `CreateProjectDto`. كل نوع له كلاسه.
