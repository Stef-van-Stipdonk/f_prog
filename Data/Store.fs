namespace Rommulbad.Data.Store

open System
open Rommulbad.Data.Database

/// Here a store is created that contains the following tables with the following attributes
///
/// candidates (primary key is name)
/// - name (consists of words seperated by spaces)
/// - date of birth
/// - guardian id (see guardian id)
/// - highest swimming diploma (A, B, or C, with C being the highest)
///
/// sessions (primary key is compound: candidate name and date)
/// - candidate name (foreign key to employees)
/// - date
/// - minutes(int)
///
/// guardians
/// - id (3 digits followed by dash and 4 letters, e.g. 133-LEET)
/// - name (consists of words separated by spaces)
type Store() =
    member val candidates =
        [ "Eleanor", DateTime(2024, 2, 2), "123-ABCD", "A"
          "Camiel", DateTime(2024, 2, 2), "123-ABCD", "C"
          "Lore", DateTime(2024, 2, 2), "999-ZZZZ", ""
          "Lena", DateTime(2024, 2, 2), "234-FDEG", "B"
        ]
        |> Seq.map (fun (n, bd, gi, dpl) ->
            let name = match Domain.Candidate.Name.ofRaw n with
                        | Ok name -> name
                        | Error e -> failwith e
                        
            let bd = match Domain.Candidate.DateOfBirth.ofRaw bd with
                        | Ok bd -> bd
                        | Error e -> failwith e
                        
            let gi = match Domain.Candidate.GuardianId.ofRaw gi with
                        | Ok gi -> gi
                        | Error e -> failwith e
                        
            let dpl = match Domain.Candidate.Diploma.ofRaw dpl with
                        | Ok dpl -> dpl
                        | Error e -> failwith e
            (name, { Domain.Candidate.Name = name; Domain.Candidate.DateOfBirth = bd; Domain.Candidate.GuardianId = gi; Domain.Candidate.Diploma = dpl }))
        |> InMemoryDatabase.ofSeq

    member val sessions =
        [ ("Eleanor", false, DateTime(2024, 2, 2), 3)
          ("Eleanor", false, DateTime(2024, 3, 2), 5)
          ("Eleanor", false, DateTime(2024, 3, 2), 10)
          ("Eleanor", true, DateTime(2024, 4, 1), 30)
          ("Eleanor", true, DateTime(2024, 5, 2), 10)
          ("Eleanor", true, DateTime(2024, 5, 3), 15)
          ("Camiel", false, DateTime(2023, 4, 10), 15)
          ("Camiel", true, DateTime(2023, 4, 17), 10)
          ("Camiel", true, DateTime(2023, 5, 24), 20)
          ("Camiel", true, DateTime(2023, 5, 14), 10)
          ("Camiel", true, DateTime(2023, 6, 13), 20)
          ("Camiel", true, DateTime(2023, 6, 17), 10)
          ("Camiel", true, DateTime(2023, 7, 10), 20)
          ("Camiel", true, DateTime(2023, 7, 17), 10)
          ("Camiel", true, DateTime(2023, 8, 10), 20)
          ("Camiel", true, DateTime(2023, 8, 17), 10)
          ("Camiel", true, DateTime(2023, 9, 10), 20)
          ("Camiel", true, DateTime(2023, 9, 17), 10)
          ("Camiel", true, DateTime(2023, 10, 10), 20)
          ("Camiel", true, DateTime(2023, 10, 17), 10)
          ("Camiel", true, DateTime(2023, 11, 10), 20)
          ("Camiel", true, DateTime(2023, 11, 17), 10)
          ("Camiel", true, DateTime(2023, 12, 10), 20)
          ("Camiel", true, DateTime(2023, 12, 17), 10)
          ("Lore", false, DateTime(2024, 6, 3), 1)
          ("Lore", false, DateTime(2024, 6, 10), 5)
          ("Lena", true, DateTime(2024, 6, 3), 10)
          ("Lena", false, DateTime(2024, 6, 4), 30)
          ("Lena", true, DateTime(2024, 6, 5), 16)
          ("Lena", false, DateTime(2024, 6, 6), 30)
          ("Lena", false, DateTime(2024, 6, 7), 30)
          ("Lena", false, DateTime(2024, 6, 8), 30)
        ]
        |> Seq.map (fun (n, deep, date, min) ->
            let name = match Domain.Session.NameOfCandidate.ofRaw n with
                        | Ok name -> name
                        | Error e -> failwith e
            let sessionDate = match Domain.Session.DateOfSession.ofRaw date with
                              | Ok date -> date
                              | Error e -> failwith e
            let isDeep = match Domain.Session.SwamInDeepPool.ofRaw deep with
                         | Ok deep -> deep
                         | Error e -> failwith e
            let minutes = match Domain.Session.MinutesSwam.ofRaw min with
                          | Ok minutes -> minutes
                          | Error e -> failwith e

            ((name, date), { Domain.Session.NameOfCandidate = name; Domain.Session.DateOfSession = sessionDate; Domain.Session.SwamInDeepPool = isDeep; Domain.Session.MinutesSwam = minutes }))
        |> InMemoryDatabase.ofSeq

    member val guardians =
        [ "123-ABCD", "Jan Janssen"
          "234-FDEG", "Marie Moor"
          "999-ZZZZ", "Margeet van Lankerveld"
          "234-FDEG", "Marie Moor"
          ]
        |> Seq.map (fun t ->
            let id = match Domain.Guardian.Id.ofRaw (fst t) with
                     | Ok id -> id
                     | Error e -> failwith e
            let name = match Domain.Guardian.Name.ofRaw (snd t) with
                       | Ok name -> name
                       | Error e -> failwith e
            
            fst t, { Domain.Guardian.Id = id; Domain.Guardian.Name = name; })
        |> InMemoryDatabase.ofSeq
