#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let lines = 
    readLines "input.txt"
    |> fun x -> printfn "%A" x; x