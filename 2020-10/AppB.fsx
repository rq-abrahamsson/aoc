#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let rec getPossibleNextStep (s : int list) =
    let head :: tail = s
    let tail = tail |> List.toSeq
    if s |> List.isEmpty || tail |> Seq.isEmpty then
        1
    else 
        tail 
        |> Seq.takeWhile (fun x -> x <= head + 3)
        |> Seq.map (fun x -> (x, tail |> Seq.skipWhile (fun y -> y < x)))
        |> Seq.map (fun (_, x) -> x |> Seq.toList)
        |> Seq.map getPossibleNextStep
        |> Seq.sum


let result = 
    readLines "input-test.txt"
    |> Seq.map int
    |> Seq.sort
    |> List.ofSeq
    |> (fun x -> 0 :: (x @ [x.[List.length x - 1] + 3]))
    |> getPossibleNextStep


printfn "%A" result
