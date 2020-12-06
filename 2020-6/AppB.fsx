#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let groupedAnswers lines =
    lines
    |> List.fold (fun (newList : string list list, builtList : string list) (elem : string) ->
        if elem = "" then
            (builtList :: newList, [])
        else
            (newList, elem :: builtList)
    ) (List.empty, List.empty)
    |> fun (answers, lastAnswer) -> lastAnswer :: answers

readLines "input.txt"
|> Seq.toList
|> groupedAnswers
|> List.map (fun x -> (x |> String.concat "" |> Set.ofSeq |> Set.toList, x))
|> List.map (
    fun (avaliableChars, answers) ->
        avaliableChars |> List.filter (fun char ->
            answers |> List.forall (fun x -> x |> String.exists (fun y -> y = char))
        )
    )
|> List.map List.length
|> List.sum
|> printfn "%A"
