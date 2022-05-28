#r "nuget: FSharp.Json"

module PokeApiModule = 
    
    open System.Net.Http

    module private HttpClientModule =
        
        let GetAsync (client:HttpClient) (endpoint:string) =
            async {
                let! response = client.GetAsync(endpoint) |> Async.AwaitTask
                response.EnsureSuccessStatusCode() |> ignore
                let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                return content
            }
    
    let private _baseUrl = "https://pokeapi.co/api/v2/"
    
    let GetPokemon dexNumber =
        async {                
            use client = new HttpClient()

            let endpoint = _baseUrl + "pokemon/" + dexNumber

            let! pkmn = HttpClientModule.GetAsync client endpoint

            return pkmn
        }

type Pokemon = {
    id : int
    name : string
}

open System
open FSharp.Json

printf "Digite um número: " |> ignore
let num = Console.ReadLine()

let res = PokeApiModule.GetPokemon num
                            |> Async.RunSynchronously
                            |> Json.deserialize<Pokemon>

printf "%A : %A" res.id res.name

0