module Rommulbad.Data.SessionService

open System
open Rommulbad.Data.Database
open Rommulbad.Data.Store
open Rommulbad.Domain
open Rommulbad.Application.SessionService


type SessionService(store: Store) =
    interface ISessionService with            
        member _.AddSession(name: string, session: Session)=
            let key = (name, session.Date)
            let session = {
                Name = name
                Deep = session.Deep
                Date = session.Date
                Minutes = session.Minutes
            }
            match InMemoryDatabase.insert key session store.sessions with
            | Ok() -> Ok()
            | Error e -> Error (sprintf "Failed to add session: %s" (e.ToString()))

        member _.GetSessions(name: string) =
            store.sessions
            |> InMemoryDatabase.filter (fun {
                                             Name = n
                                             } -> n = name)

        member _.GetTotalMinutes(name: string) =
            store.sessions
            |> InMemoryDatabase.filter (fun {
                                             Name = n
                                             } -> n = name)
            |> Seq.map (fun {
                             Deep = _
                             Date = _
                             Name = _   
                             Minutes = minutes
                             } -> minutes)
            |> Seq.sum

        member _.GetEligibleSessions(name: string, diploma: string) =
            let shallowOk =
                match diploma with
                | "A" -> true
                | _ -> false

            let minMinutes =
                match diploma with
                | "A" -> 1
                | "B" -> 10
                | _ -> 15

            let filter {Name = n
                        Deep = deep
                        Date = _
                        Minutes = minutes} = (n = name) && (deep || shallowOk) && (minutes >= minMinutes)

            store.sessions
            |> InMemoryDatabase.filter filter

        member _.GetTotalEligibleMinutes(name: string, diploma: string) =
            let shallowOk =
                match diploma with
                | "A" -> true
                | _ -> false

            let minMinutes =
                match diploma with
                | "A" -> 1
                | "B" -> 10
                | _ -> 15

            let filter {Name = n
                        Deep = deep
                        Date = _
                        Minutes = minutes} = (n = name) && (deep || shallowOk) && (minutes >= minMinutes)

            store.sessions
            |> InMemoryDatabase.filter filter
            |> Seq.map (fun {
                            Minutes = minutes
                            } -> minutes)
            |> Seq.sum