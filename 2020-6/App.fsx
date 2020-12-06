#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let groupedAnswers lines =
    lines
    |> List.fold (fun (newList : string list, builtString : string) (elem : string) ->
        if elem = "" then
            (builtString :: newList, "")
        else
            (newList, (sprintf "%s%s" builtString elem).Trim([|' '|]))
    ) (List.empty, "")
    |> fun (answers, lastAnswer) -> lastAnswer :: answers

readLines "input.txt"
|> Seq.toList
|> groupedAnswers
|> List.map Set.ofSeq
|> List.map Set.toList
|> List.map List.length
|> List.sum
|> printfn "%A"
