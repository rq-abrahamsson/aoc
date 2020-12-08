#r "nuget: FSharpPlus"
open FSharpPlus
open System

let readLines filePath = System.IO.File.ReadLines(filePath);

let getStructuredTuple [x;y] =
    (String.trimEnd ['s'] x, 
        y 
        |> String.split [ ", " ] 
        |> Seq.toList 
        |> List.map (String.trimEnd ['s'])
        |> List.map (String.split [" "])
        |> List.map Seq.toList
        |> List.map (fun x -> ((if x.Head = "no" then 0 else int x.Head), x.Tail))
        |> List.map (fun (x, y) -> (x, String.concat " " y))
        )

let getBetterStructure (x, contained) =
    contained
    |> List.map (fun (number, y) -> (x, number, y) )

let findChildren (input : string) (data : (string * int * string) list) =
    data
    |> List.filter (fun (x, _, _) -> x = input)
    |> List.map (fun (_, amount, bag) -> (amount, bag))

let rec findSolution data input =
    let children = 
        findChildren input data 
    if children.IsEmpty || children |> List.forall (fun (amount, _) -> amount = 0) then
        1
    else
        children 
        |> List.map (fun (amount, bag) -> amount * findSolution data bag)
        |> List.sum
        |> fun x -> x + 1


let data = 
    readLines "input.txt"
    |> Seq.map (fun x -> x |> String.split [ "contain" ] |> Seq.toList)
    |> Seq.map (List.map String.trimWhiteSpaces)
    |> Seq.map (List.map (String.trimEnd ['.']))
    |> Seq.map getStructuredTuple
    |> Seq.map getBetterStructure
    |> Seq.concat
    |> Seq.toList


let result =
    findSolution data "shiny gold bag" - 1

printfn "%A" result

