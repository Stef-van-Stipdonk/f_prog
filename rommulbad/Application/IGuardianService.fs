namespace Rommulbad.Application.GuardianService

    open Domain.Guardian

    type IGuardianService =
        abstract member AddGuardian: Id * Name -> Option<string>
        
