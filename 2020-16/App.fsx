#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let lines = 
    readLines "input.txt"
    |> Seq.toList

type Rule = 
    {
        gt: int
        lt: int
    }

let toRule (r : string list) =
    {
        gt = int r.[0]
        lt = int r.[1]
    }

let parseRule rule = 
    rule
    |> String.split [":"; " "]
    |> Seq.toList
    |> List.skipWhile (fun x -> x <> "")
    |> List.skip 1
    |> List.filter (fun x -> x <> "or")
    |> List.map (fun x -> x |> String.split ["-"])
    |> List.map Seq.toList
    |> List.map toRule

let parseTicket str =
    str
    |> String.split [","]
    |> Seq.toList
    |> List.map int

let validateTicket rules ticket =
    let mutable errorNumbers = []
    ticket |> List.forall (fun ticketNumber -> 
        rules |> List.exists (fun rule ->
            rule.gt <= ticketNumber && ticketNumber <= rule.lt 
        )
        |> (fun x -> 
            if x = false then
                errorNumbers <- ticketNumber::errorNumbers
                x
            else
                x
            )
    )
    errorNumbers

let rules =
    lines
    |> List.takeWhile (fun x -> x <> "")
    |> List.map parseRule
    |> List.concat

let yourTicket =
    lines
    |> List.skipWhile (fun x -> x <> "your ticket:")
    |> List.skip 1
    |> List.head
    |> parseTicket
    |> validateTicket rules

let nearbyTickets =
    lines
    |> List.skipWhile (fun x -> x <> "nearby tickets:")
    |> List.skip 1
    |> List.map parseTicket
    |> List.map (validateTicket rules)
    |> List.concat
    |> List.sum
    |> fun x -> printfn "%A" x; x