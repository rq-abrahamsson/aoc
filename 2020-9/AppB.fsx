#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let numberToFind = 133015568L

let rec testNumber (list : int64 list) (summedNumbers : int64 list) (sum : int64) =
    let newSum = sum + list.Head
    if newSum = numberToFind then
        Some (list.Head :: summedNumbers)
    else if newSum > numberToFind || list.Tail |> List.isEmpty then
        None
    else
        testNumber list.Tail (list.Head :: summedNumbers) newSum

let rec testNumbers (list : int64 list) =
    let result = testNumber list [] 0L
    match result with
    | None -> testNumbers list.Tail
    | Some value -> value


let data = 
    readLines "input.txt"
    |> Seq.toList
    |> List.map (fun x -> int64 x)

testNumbers data
|> fun l -> (List.max l) + (List.min l)
|> printfn "%A"



