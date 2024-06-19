module Rommulbad.Service.HttpHandlers

open Giraffe
open Rommulbad.Service

let requestHandlers : HttpHandler = 
    choose [
        Candidate.routes
        Session.routes
        Guardian.routes
    ]