namespace Rommulbad.Application.GuardianService

    open Domain
    open Domain.Candidate
    open Domain.Guardian

    type IGuardianService =
        abstract member AddGuardian: Guardian -> Result<unit, string>
        
        abstract member AddCandidateForGuardian: Id * Candidate -> Result<unit, string>
        
