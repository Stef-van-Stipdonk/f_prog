module Rommulbad.Data.SessionService

open System
open Domain.Candidate
open Domain.Session
open Rommulbad.Data.Database
open Rommulbad.Data.Store
open Rommulbad.Application.SessionService

type SessionService(store: Store) =
    interface ISessionService with            
        member _.AddSession(name: NameOfCandidate, session: Session)=
            let key = (name, DateOfSession.raw session.DateOfSession)
            let session = {
                NameOfCandidate = name
                SwamInDeepPool = session.SwamInDeepPool
                DateOfSession = session.DateOfSession
                MinutesSwam = session.MinutesSwam
            }
            
            match InMemoryDatabase.insert key session store.sessions with
                | Ok() -> Ok()
                | Error e -> Error $"Failed to add session: %s{e.ToString()}"            

        member _.GetSessions(name: NameOfCandidate) =
            store.sessions
            |> InMemoryDatabase.filter (fun {
                                             NameOfCandidate = n
                                             } -> n = name)

        member _.GetTotalMinutes(name: NameOfCandidate) =
            store.sessions
            |> InMemoryDatabase.filter (fun {
                                             NameOfCandidate = n
                                             } -> n = name)
            |> Seq.map (fun {
                             SwamInDeepPool = _
                             DateOfSession = _
                             NameOfCandidate = _   
                             MinutesSwam = minutes
                             } -> MinutesSwam.raw minutes)
            |> Seq.sum

        member _.GetEligibleSessions(name: NameOfCandidate, diploma: Diploma) =
            let diplomaValue = Diploma.raw diploma
            
            let shallowOk =
                match diplomaValue with
                | "A" -> true
                | _ -> false

            let minMinutes =
                match diplomaValue with
                | "A" -> 1
                | "B" -> 10
                | _ -> 15

            let filter {NameOfCandidate = n
                        SwamInDeepPool = deep
                        DateOfSession = _
                        MinutesSwam = minutes} = (n = name) && (SwamInDeepPool.raw deep || shallowOk) && (MinutesSwam.raw minutes >= minMinutes)

            store.sessions
            |> InMemoryDatabase.filter filter
                
        member _.GetTotalEligibleMinutes(name: NameOfCandidate, diploma: Diploma) =
            let diplomaValue = Diploma.raw diploma

            let shallowOk =
                match diplomaValue with
                | "A" -> true
                | _ -> false

            let minMinutes =
                match diplomaValue with
                | "A" -> 1
                | "B" -> 10
                | _ -> 15

            let filter {NameOfCandidate = n
                        SwamInDeepPool = deep
                        DateOfSession = _
                        MinutesSwam = minutes} = (n = name) && (SwamInDeepPool.raw deep || shallowOk) && (MinutesSwam.raw minutes >= minMinutes)

            store.sessions
            |> InMemoryDatabase.filter filter
            |> Seq.map (fun {
                            MinutesSwam = minutes
                            } -> MinutesSwam.raw minutes)
            |> Seq.sum
            
        member this.AwardDiploma(rawName: NameOfCandidate) =
            match (Name.ofRaw (NameOfCandidate.raw rawName)) with
            | Error e -> Error e
            | Ok name ->
                match InMemoryDatabase.lookup name store.candidates with
                    | Some candidate ->
                        let nextDiploma =
                            match Diploma.raw candidate.Diploma with
                            | "" -> "A"
                            | "A" -> "B"
                            | "B" -> "C"
                            | "C" -> "No more diplomas"
                            | _ -> failwith "Invalid diploma value in database"
                            
                        match nextDiploma with
                        | "No more diplomas" -> Error "No more diplomas to award, you have caught them all *Pokemon theme song*"
                        | rawDiploma ->
                            match Diploma.ofRaw rawDiploma with
                            | Error e -> Error e
                            | Ok diploma ->
                                let inst = this :> ISessionService
                                match
                                    inst.GetTotalEligibleMinutes(rawName, diploma) > 120 && nextDiploma = "A"
                                    || inst.GetTotalEligibleMinutes(rawName, diploma) > 150 && nextDiploma = "B"
                                    || inst.GetTotalEligibleMinutes(rawName, diploma) > 180 && nextDiploma = "C" with
                                | true ->
                                    let updatedCandidate = {
                                        Name = candidate.Name
                                        DateOfBirth = candidate.DateOfBirth
                                        GuardianId = candidate.GuardianId
                                        Diploma = diploma
                                    }
                                    
                                    match InMemoryDatabase.update name updatedCandidate store.candidates with
                                        | _ -> Ok $"Diploma awarded: %s{rawDiploma}"

                                | false -> Ok $"Not enough minutes to award diploma %s{rawDiploma} yet"
                                    