#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let getNextSegment (input : char) ((lower, higher): int * int) =
    let middle = (higher - lower) / 2
    if input = 'F' || input = 'L' then
        (lower, lower + middle)
    else if input = 'B' || input = 'R' then
        (lower + middle + 1, higher)
    else 
        (lower, higher)

let rec getRow (init: int * int) (chars : char list) =
    let (lower, higher) = getNextSegment chars.Head init
    if lower = higher then
        lower
    else
        getRow (lower, higher) chars.Tail 

let getRow128 = getRow (0, 127)
let getColumn8 = getRow (0, 7)

let getSeatId (input : char List) =
    let (first, last) = input |> List.splitAt 7
    let row = getRow128 first
    let column = getColumn8 last
    (row * 8) + column 

// Run
readLines "input.txt"
|> Seq.toList 
|> List.map Seq.toList
|> List.map getSeatId
|> List.max
|> printfn "%A"