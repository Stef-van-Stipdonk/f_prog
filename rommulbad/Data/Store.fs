namespace Rommulbad.Data.Store

open System
open Rommulbad.Data.Database
open Rommulbad.Domain

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
    member val candidates: InMemoryDatabase<string, Candidate> =
        [ "Eleanor", DateTime(2024, 2, 2), "123-ABCD", "A"
          "Camiel", DateTime(2024, 2, 2), "123-ABCD", "C"
          "Lore", DateTime(2024, 2, 2), "9999-ZZZ", ""
          "Lena", DateTime(2024, 2, 2), "234-FDEG", "B"
        ]
        |> Seq.map (fun (n, bd, gi, dpl) -> (n, { Name = n; DateOfBirth = bd; GuardianId = gi; Diploma = dpl }))
        |> InMemoryDatabase.ofSeq

    member val sessions: InMemoryDatabase<string * DateTime, Session> =
        [ "Eleanor", false, DateTime(2024, 2, 2), 3
          "Eleanor", false, DateTime(2024, 3, 2), 5
          "Eleanor", false, DateTime(2024, 3, 2), 10
          "Eleanor", true, DateTime(2024, 4, 1), 30
          "Eleanor", true, DateTime(2024, 5, 2), 10
          "Eleanor", true, DateTime(2024, 5, 3), 15
          "Camiel", false, DateTime(2023, 4, 10), 15
          "Camiel", true, DateTime(2023, 4, 17), 10
          "Camiel", true, DateTime(2023, 5, 24), 20
          "Camiel", true, DateTime(2023, 5, 14), 10
          "Camiel", true, DateTime(2023, 6, 13), 20
          "Camiel", true, DateTime(2023, 6, 17), 10
          "Camiel", true, DateTime(2023, 7, 10), 20
          "Camiel", true, DateTime(2023, 7, 17), 10
          "Camiel", true, DateTime(2023, 8, 10), 20
          "Camiel", true, DateTime(2023, 8, 17), 10
          "Camiel", true, DateTime(2023, 9, 10), 20
          "Camiel", true, DateTime(2023, 9, 17), 10
          "Camiel", true, DateTime(2023, 10, 10), 20
          "Camiel", true, DateTime(2023, 10, 17), 10
          "Camiel", true, DateTime(2023, 11, 10), 20
          "Camiel", true, DateTime(2023, 11, 17), 10
          "Camiel", true, DateTime(2023, 12, 10), 20
          "Camiel", true, DateTime(2023, 12, 17), 10
          "Lore", false, DateTime(2024, 6, 3), 1
          "Lore", false, DateTime(2024, 6, 10), 5
          "Lena", true, DateTime(2024, 6, 3), 10
          "Lena", false, DateTime(2024, 6, 4), 30
          "Lena", true, DateTime(2024, 6, 5), 16
          "Lena", false, DateTime(2024, 6, 6), 30
          "Lena", false, DateTime(2024, 6, 7), 30
          "Lena", false, DateTime(2024, 6, 8), 30
          ]
        |> Seq.map (fun (n, deep, date, min) -> (n, date), { Name = n; Date = date; Deep = deep; Minutes = min })
        |> InMemoryDatabase.ofSeq

    member val guardians: InMemoryDatabase<string, Guardian> =
        [ "123-ABCD", "Jan Janssen"
          "234-FDEG", "Marie Moor"
          "999-ZZZZ", "Margeet van Lankerveld"
          "234-FDEG", "Marie Moor"
          ]
        |> Seq.map (fun t -> fst t, { Id = fst t; Name = snd t; Candidates = [] })
        |> InMemoryDatabase.ofSeq
