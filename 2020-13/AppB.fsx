#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);


let isAScheduledTime t =
    fun (``t+``, interval) -> 
        let time = t + ``t+``
        time - interval * (time / interval) <> 0L


let findT (intervalT0 : int64) (rest : (int64 * int64) list) =
    Seq.initInfinite (fun x -> (int64 x) * intervalT0)
    |> Seq.skipWhile (fun t -> rest |> List.exists (isAScheduledTime t))
    |> Seq.take 1
    |> Seq.head

let normalizeForFirst (list : (int64 * int64) list) =
    let (decreaseNumber, _) = list.Head
    list
    |> List.map (fun (x, y) -> (x - decreaseNumber, y))

let parseBusTimes s =
    s 
    |> String.split [","]
    |> Seq.toList
    |> List.mapi (fun i x -> (i, x))
    |> List.filter (fun (_, x) -> x <> "x")
    |> List.map (fun (x, y) -> (int64 x, int64 y))

readLines "input.txt"
|> Seq.toList
|> fun [x; y] -> parseBusTimes y
|> List.sortWith (fun (_, x) (_, y) -> int (y - x))
|> normalizeForFirst
|> fun x -> printfn "%A" x; x
|> fun ((_, v)::tail) -> findT v tail
|> fun x -> printfn "%A" x; x