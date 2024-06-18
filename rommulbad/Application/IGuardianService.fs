namespace Rommulbad.Application.GuardianService

    type IGuardianService =
        abstract member AddGuardian: string * string -> Option<string>
        
