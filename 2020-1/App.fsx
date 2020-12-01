#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let lines = readLines "input.txt"

let lineList = lines |> Seq.toList |> List.map int

let multiplied = 
    lineList |> List.mapi (fun i x -> 
        lineList |> List.mapi (fun j y -> 
            if i <> j then
                (x, y)
            else
                (0, 0)
            )
        )
    |> List.concat
    |> Set.ofList
    |> Set.toList
    |> List.map (fun (l, r) -> (l, r, l + r, l * r))
    |> List.filter (fun (_, _, x, _) -> x = 2020)
    |> List.map (fun (_, _, _, x) -> x)
    |> List.head
printfn "%A" multiplied