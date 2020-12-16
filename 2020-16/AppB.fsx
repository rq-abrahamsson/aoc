#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

type Rule = 
    {
        name: string
        gt1: int
        lt1: int
        gt2: int
        lt2: int
    }

let toRule ((name, r) : (string * (string list))) =
    {
        name = name
        gt1 = int r.[0]
        lt1 = int r.[1]
        gt2 = int r.[2]
        lt2 = int r.[3]
    }

let parseRule rule = 
    rule
    |> String.split [":"; " "]
    |> Seq.toList
    |> fun y -> (
        y
        |> List.takeWhile (fun x -> x <> "")
        |> String.concat " "
        ,
        y 
        |> List.skipWhile (fun x -> x <> "")
        |> List.tail
        |> List.filter (fun x -> x <> "or")
        |> List.map (fun x -> x |> String.split ["-"])
        |> List.map Seq.toList
        |> List.concat
        )
    |> toRule

let parseTicket str =
    str
    |> String.split [","]
    |> Seq.toList
    |> List.map int

let validateTicket rules ticket =
    ticket |> List.forall (fun ticketNumber -> 
        rules |> List.exists (fun rule ->
            (rule.gt1 <= ticketNumber &&
            ticketNumber <= rule.lt1) ||
            (rule.gt2 <= ticketNumber &&
            ticketNumber <= rule.lt2)
        )
    )

let findClassForRow rules index (tickets : int list list) =
    rules
    |> List.filter (fun rule ->
        tickets
        |> List.forall (fun (ticket : int list) ->
            (rule.gt1 <= ticket.[index] &&
            ticket.[index] <= rule.lt1) ||
            (rule.gt2 <= ticket.[index] &&
            ticket.[index] <= rule.lt2)
        )
    )

let findPossibleClasses rules (tickets : int list list) =
    let ticketNumbersLength = (tickets.[0] |> List.length) - 1
    [0..ticketNumbersLength]
    |> List.map (fun index -> 
        findClassForRow rules index tickets
    )

let rec placeSingleRules (rules : Rule list list) =
    if rules |> List.forall (fun r -> r |> List.length = 1) then
        rules |> List.concat
    else
        let singleRules = rules |> List.filter (fun l -> l |> List.length = 1) |> List.concat
        rules |> List.map (fun l -> 
            if l |> List.length = 1 then
                l
            else 
                l |> List.except singleRules
            )
        |> placeSingleRules

let findClasses rules (tickets : int list list) =
    findPossibleClasses rules tickets
    |> placeSingleRules

let lines = 
    readLines "input.txt"
    |> Seq.toList

let rules =
    lines
    |> List.takeWhile (fun x -> x <> "")
    |> List.map parseRule

let nearbyTickets =
    lines
    |> List.skipWhile (fun x -> x <> "nearby tickets:")
    |> List.skip 1
    |> List.map parseTicket
    |> List.filter (validateTicket rules)
    |> findClasses rules
    |> List.map (fun x -> x.name)

let yourTicket =
    lines
    |> List.skipWhile (fun x -> x <> "your ticket:")
    |> List.skip 1
    |> List.head
    |> parseTicket

let answer =
    List.map2 (fun x y -> (x, y)) nearbyTickets yourTicket
    |> List.filter (fun (name, _) -> name |> String.startsWith "departure")
    |> List.map (fun (_, value) -> int64 value)
    |> List.fold (fun acc elem -> acc * elem) 1L
    |> fun x -> printfn "%A" x; x

