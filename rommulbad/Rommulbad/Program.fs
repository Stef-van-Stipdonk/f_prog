open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Rommulbad.Application.CandidateService
open Rommulbad.Application.GuardianService
open Rommulbad.Application.SessionService
open Rommulbad.Data
open Rommulbad.Data.CandidateService
open Rommulbad.Data.GuardianService
open Rommulbad.Data.SessionService
open Rommulbad.Data.Store
open Thoth.Json.Giraffe
open Thoth.Json.Net
open Rommulbad.Service.HttpHandlers

let configureApp (app: IApplicationBuilder) =
    app.UseGiraffe requestHandlers

let configureServices (services: IServiceCollection) =
    services
        .AddGiraffe()
        .AddSingleton<Store>(Store())
        .AddSingleton<ICandidateService, CandidateService>()
        .AddSingleton<IGuardianService, GuardianService>()
        .AddSingleton<ISessionService, SessionService>()
        .AddSingleton<Json.ISerializer>(ThothSerializer(skipNullField = false, caseStrategy = CaseStrategy.CamelCase))
    |> ignore

[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder.Configure(configureApp).ConfigureServices(configureServices)
            |> ignore)
        .Build()
        .Run()

    0
