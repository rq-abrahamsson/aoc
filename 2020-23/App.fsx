#r "nuget: FSharpPlus"
open FSharpPlus

// let cups = "389125467"
// let nrMoves = 10
let cups = "167248359"
let nrMoves = 100

module Seq =
    let rec repeat items =
        seq {
            yield! items
            yield! repeat items
        }

    let takeWhileInclusive predicate l =
        seq { 
            yield! Seq.takeWhile predicate l
            yield! l |> Seq.skipWhile predicate |> Seq.truncate 1 
        }

let rec findDestinationCup label labelSeq =
    let labelToFind =
        if label = 1 then
            9
        else
            label - 1
    match labelSeq |> Seq.tryFind (fun x -> x = labelToFind) with
    | Some v -> v
    | None -> findDestinationCup labelToFind labelSeq

let startingIntegerSequence = 
    cups
    |> Seq.toList
    |> List.map (string >> int)
    |> List.toSeq
    |> Seq.repeat

let makeMove labelSeq =
    let currentCup = labelSeq |> Seq.head
    let pickedUpCups = labelSeq |> Seq.skip 1 |> Seq.take 3
    let restCups = labelSeq |> Seq.skip 4 |> Seq.takeWhileInclusive (fun x -> x <> currentCup)
    let restCupsList = restCups |> Seq.toList
    let destinationCup = findDestinationCup currentCup restCups
    let destinationCupIndex = restCupsList |> List.findIndex (fun x -> x = destinationCup)

    restCupsList
    |> List.splitAt (destinationCupIndex + 1)
    |> fun (x, y) -> List.concat [x; pickedUpCups |> Seq.toList; y]
    |> List.toSeq
    |> Seq.repeat
    |> Seq.skipWhile (fun x -> x <> currentCup)
    |> Seq.skip 1

let getAnswer labelSeq =
    labelSeq
    |> Seq.skipWhile (fun x -> x <> 1)
    |> Seq.skip 1
    |> Seq.take 8
    |> Seq.toList
    |> List.map string
    |> String.concat ""
    |> fun x -> printfn "%A" x; x

let rec makeAllMoves cups count =
    [0..count-1]
    |> List.fold (fun acc _ -> makeMove acc) cups
    |> getAnswer

makeAllMoves startingIntegerSequence nrMoves 