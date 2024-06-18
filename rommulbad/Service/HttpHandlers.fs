module Rommulbad.Service.HttpHandlers

open Giraffe
let requestHandlers : HttpHandler = 
    choose [
        Candidate.routes
        Session.routes
    ]