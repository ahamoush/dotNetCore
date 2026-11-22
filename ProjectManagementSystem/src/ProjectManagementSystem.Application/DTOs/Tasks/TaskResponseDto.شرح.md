# شرح TaskResponseDto.cs

شكل المهمة الراجع للعميل.

- `Id` رقم المهمة.
- `Title` العنوان.
- `Description` الوصف.
- `IsCompleted` هل اكتملت.
- `DueDate` الموعد أو `null`.
- `ProjectId` رقم المشروع. نرجع الرقم فقط، لا كائن المشروع كاملاً.
