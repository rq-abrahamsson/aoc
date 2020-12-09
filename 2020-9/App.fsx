#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

module List =
    let combinations list =
        list |> List.map (fun x ->
            list |> List.map (fun y ->
                if x <> y then
                    (x, y)
                else
                    (0L, 0L)
            )
        )

let getPossibleNumbers (numbers : int64 list) =
    numbers
    |> List.combinations
    |> List.concat
    |> List.filter (fun (x, y) -> x <> 0L && y <> 0L)
    |> List.map (fun (x, y) -> x + y)
    |> Set.ofList
    |> Set.toList

let rec getErrorNumber (nextPreambleNumbers : int64 list) (nextRestNumbers : int64 list) =
    let possibleNumbers = nextPreambleNumbers |> getPossibleNumbers
    if possibleNumbers |> List.contains nextRestNumbers.[0] then
        let nextPreambleNumbers = (nextPreambleNumbers |> List.drop 1) @ [nextRestNumbers.[0]]
        let nextRestNumbers = nextRestNumbers |> List.drop 1
        getErrorNumber nextPreambleNumbers nextRestNumbers
    else if nextRestNumbers |> List.isEmpty then
        0L
    else
        nextRestNumbers.[0]

let preamble = 25

let data = 
    readLines "input.txt"
    |> Seq.toList
    |> List.map (fun x -> int64 x)

let start = 
    data
    |> List.take preamble

let rest = 
    data
    |> List.drop preamble

getErrorNumber start rest
|> printfn "%A"