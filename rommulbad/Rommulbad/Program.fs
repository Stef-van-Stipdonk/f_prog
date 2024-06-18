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

let configureApp (app: IApplicationBuilder) =
    let candidateService = app.ApplicationServices.GetService<ICandidateService>()
    let guardianService = app.ApplicationServices.GetService<IGuardianService>()
    let sessionService = app.ApplicationServices.GetService<ISessionService>()
    app.UseGiraffe(Rommulbad.Service.Web.routes candidateService sessionService guardianService)

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
