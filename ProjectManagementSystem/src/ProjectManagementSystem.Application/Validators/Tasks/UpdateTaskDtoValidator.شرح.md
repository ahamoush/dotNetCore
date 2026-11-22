# شرح UpdateTaskDtoValidator.cs

- `Title`: من 3 إلى 150 وغير فارغ.
- `Description`: حتى 1000.

لا قاعدة على `IsCompleted` لأنها `bool` وقيمتها الوحيدة نعم أو لا.
لا قاعدة على `DueDate`. غيابه مسموح.
