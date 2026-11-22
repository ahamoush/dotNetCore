# شرح UpdateProjectDto.cs

جسم طلب تعديل مشروع. الرقم يأتي من عنوان الرابط `PUT /api/projects/5` وليس من هذا الكائن.

- `public string Name`
  الاسم الجديد. مطلوب وطوله من 3 إلى 100 في الـ Validator.

- `public string Description`
  الوصف الجديد. أقصى طول 500.

التعديل لا يرسل الحالة ولا التواريخ. الخدمة تنسخ الاسم والوصف فقط، ثم تضع `UpdatedAt` بنفسها.
