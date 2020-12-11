#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let differences = 
    readLines "input.txt"
    |> Seq.map int
    |> Seq.sort
    |> List.ofSeq
    |> (fun x -> 0 :: (x @ [x.[List.length x - 1] + 3]))
    |> List.fold (fun (l, prevElem) curr -> 
        if prevElem = -1 then
            (l, curr)
        else
            ((prevElem, curr) :: l, curr)
    ) ([], -1)
    |> (fun (x, _) -> x)
    |> List.rev
    |> List.map (fun (x, y) -> y - x)

let result = 
    (differences |> List.sumBy (fun x -> if x = 1 then 1 else 0))
    *
    (differences |> List.sumBy (fun x -> if x = 3 then 1 else 0))


printfn "%A" result
