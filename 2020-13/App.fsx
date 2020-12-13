#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let getNextTimeAfterArrival arrival interval =
    Seq.initInfinite (fun x -> x * interval)
    |> Seq.skipWhile (fun x -> x < arrival)
    |> Seq.take 1
    |> Seq.head

let parseBusTimes s =
    s 
    |> String.split [","]
    |> Seq.toList
    |> List.filter (fun x -> x <> "x")
    |> List.map int

readLines "input.txt"
|> Seq.toList
|> fun [x; y] -> (int x, parseBusTimes y)
|> fun (x, y) -> (x, y |> List.map (fun z -> (z, getNextTimeAfterArrival x z)))
|> fun (arrival, y) -> y |> List.map (fun (busNr, departure) -> (busNr, departure - arrival))
|> List.minBy (fun (busNr, waitTime) -> waitTime)
|> fun (x, y) -> x * y
|> fun x -> printfn "%A" x; x