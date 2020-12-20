#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

type Node =
    | Link of int list
    | OrLink of (int list * int list)
    | Leaf of string

let rec ruleListToMap l : Map<int, Node> =
    if l |> List.isEmpty then
        Map.empty
    else
        let (idx, value)::tail = l
        (ruleListToMap tail).Add(idx, value)

let getLinkList s =
    s 
    |> String.split [" "] 
    |> Seq.toList 
    |> List.map int

let parseRule line =
    let separated =
        line
        |> String.split [":"]
        |> Seq.toList
    let idx = separated.[0]
    let value =
        if separated.[1] |> String.contains 'a' then
            Leaf "a"
        elif separated.[1] |> String.contains 'b' then
            Leaf "b"
        elif separated.[1] |>  String.contains '|' then
            separated.[1]
            |> String.split ["|"]
            |> Seq.map (String.trim " ")
            |> Seq.toList
            |> fun a -> (a.[0], a.[1])
            |> fun (x, y) -> OrLink (x |> getLinkList, y |> getLinkList )
        else
            separated.[1]
            |> String.trim " "
            |> getLinkList
            |> Link
    (int idx, value)

let rec getCombinationsHelper (newList : string list) (l : string list list) = 
    if l |> List.length = 0 then
        newList
    else
        let updatedList =
            List.allPairs newList l.Head
            |> List.map (fun (a, b) -> a + b)
        getCombinationsHelper updatedList l.Tail

let rec getCombinations (l : string list list) = 
    if l |> List.length = 0 then
        []
    else
        getCombinationsHelper l.Head l.Tail

let rec getPossibleStringsHelper rules index =
    let root =
        rules
        |> Map.find index
    match root with
    | Link value -> 
        value 
        |> List.map (getPossibleStringsHelper rules) 
        |> getCombinations
    | OrLink (left, right) -> 
        let l =
            left 
            |> List.map (getPossibleStringsHelper rules)
            |> getCombinations
        let r =
            right 
            |> List.map (getPossibleStringsHelper rules)
            |> getCombinations
        List.concat [ l; r ]
    | Leaf v -> [v]

let getPossibleStrings rules =
    getPossibleStringsHelper rules 0

let lines =
    readLines "input.txt"
    |> Seq.toList

let rules =
    lines
    |> List.takeWhile (fun x -> x <> "")
    |> List.map parseRule
    |> ruleListToMap

let possibleStrings = 
    getPossibleStrings rules 

let messages = 
    lines
    |> List.skipWhile (fun x -> x <> "")
    |> List.tail
    |> List.filter (fun x -> possibleStrings |> List.contains x)
    |> List.length
    |> fun x -> printfn "%A" x; x
