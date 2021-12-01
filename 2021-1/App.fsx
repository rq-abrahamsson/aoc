#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let lines =
  readLines "input.txt"
  |> fun x -> printfn $"%A{x}"; x
  |> Seq.map int
  |> Seq.toList
let specialLines = List.append lines [0]

lines
  |> List.mapi (fun i e -> (e, specialLines.[i+1]))
  |> List.filter (fun (x, y) -> x < y)
  |> List.length
  |> fun x -> printfn $"%A{x}"; x

