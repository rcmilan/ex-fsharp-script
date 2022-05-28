#r "nuget: Newtonsoft.Json"

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
    
    let GetPokemon num =
        async {                
            use client = new HttpClient()

            let endpoint = _baseUrl + "pokemon/" + num

            let! pkmn = HttpClientModule.GetAsync client endpoint

            return pkmn
        }

open System
open Newtonsoft.Json.Linq

printf "Digite um número: \n" |> ignore

let num = Console.ReadLine()

let parsedResult = PokeApiModule.GetPokemon(num)
                            |> Async.RunSynchronously
                            |> JObject.Parse

for pair in parsedResult do
    Console.WriteLine ("{0} : {1}", pair.Key, pair.Value)

0