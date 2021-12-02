#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let lines =
  readLines "input.txt"
  |> fun x -> printfn $"%A{x}"; x
  |> Seq.map int
  |> Seq.toList
let specialLines = List.append lines [0; 0]

let slidingSums =
  lines
    |> List.mapi (fun i e -> (e, specialLines.[i+1], specialLines.[i+2]))
    |> List.map (fun (x, y, z) -> x + y + z)
    |> fun x -> printfn $"%A{x}"; x

let specialSlidingSums = List.append slidingSums [0]

slidingSums
  |> List.mapi (fun i e -> (e, specialSlidingSums.[i+1]))
  |> List.filter (fun (x, y) -> x < y)
  |> List.length
  |> fun x -> printfn $"%A{x}"; x
