module Domain.Session

open System

type NameOfCandidate = private | NameOfCandidate of string
type DateOfSession = private | DateOfSession of DateTime
type SwamInDeepPool = private | SwamInDeepPool of bool
type MinutesSwam = private | MinutesSwam of int

type Session =
    {
      NameOfCandidate: NameOfCandidate
      SwamInDeepPool: SwamInDeepPool
      DateOfSession: DateOfSession
      MinutesSwam: MinutesSwam }
    
module NameOfCandidate =
    let ofRaw (name: string) : Result<NameOfCandidate, string> =
        if name = null then
            Error "Name of candidate cannot be null"
        else if name.Length > 0 then
            Ok <| NameOfCandidate name
        else
            Error "Name of candidate cannot be empty"

    let raw (NameOfCandidate n) = n

module DateOfSession =
    let ofRaw (date: DateTime) : Result<DateOfSession, string> =
        if date < DateTime.Now then
            Ok <| DateOfSession date
        else
            Error "Date of session cannot be in the future"

    let raw (DateOfSession d) = d
    
module SwamInDeepPool =
    let ofRaw (deep: bool) : Result<SwamInDeepPool, string> =
        Ok <| SwamInDeepPool deep

    let raw (SwamInDeepPool d) = d
    
module MinutesSwam =
    let ofRaw (minutes: int) : Result<MinutesSwam, string> =
        if minutes <= 0 then
            Error "Minutes swam must be greater than 0"
        else if minutes > 30 then
            Error "Minutes swam cannot be more than 30"
        else
            Ok <| MinutesSwam minutes
            
    let raw (MinutesSwam m) = m
   