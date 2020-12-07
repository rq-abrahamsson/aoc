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
        |> List.map (fun x -> x.Tail)
        |> List.map (String.concat " ")
        )

let getBetterStructure (x, contained) =
    contained
    |> List.map (fun y -> (x, y) )

let findChildren input data =
    data
    |> List.filter (fun (_, x) -> x = input)

let rec findSolution data (allowedBags : string list) input =
    let children = 
        findChildren input data 
        |> List.map (fun (child, _) -> child)
    if children.IsEmpty then
        allowedBags
        |> Set.ofSeq
        |> Set.toList
    else
        children 
        |> List.map (fun x -> findSolution data (allowedBags @ children) x)
        |> List.toSeq
        |> Seq.concat
        |> Set.ofSeq
        |> Set.toList


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
    findSolution data [] "shiny gold bag"
    |> List.length

// printfn "%A" data
printfn "%A" result

