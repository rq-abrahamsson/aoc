#r "nuget: FSharpPlus"
open FSharpPlus
open System

let readLines filePath = System.IO.File.ReadLines(filePath);

type Tile = {
    // data: char list list
    edges: string list
}

let getTileIndex (s : string) =
    s
    |> String.split [" "; ":"]
    |> Seq.toList
    |> fun x -> int x.[1]


let parseTile (m : Map<int, Tile>) (l : string list) =
    let idx = l |> List.head |> getTileIndex
    let tileData = 
        l 
        |> List.tail 
        |> List.takeWhile (fun x -> x <> "")
        |> List.map Seq.toList
    let edges =
        [
            tileData.[0] |> String.Concat
            tileData.[(tileData |> List.length) - 1] |> String.Concat
            seq { for i in 0 .. tileData.Length - 1 -> tileData.[i].[0] } |> String.Concat
            seq { for i in 0 .. tileData.Length - 1 -> tileData.[i].[tileData.[i].Length - 1] } |> String.Concat
        ]
    m.Add(idx,
        {
            // data = tileData
            edges = 
                List.concat [
                    edges
                    edges |> List.map String.rev
                ]
        })
let rec parseTilesHelper (m : Map<int, Tile>) (l : string list) =
    if l |> List.length = 0 then
        m
    else
        let m = parseTile m (l |> List.takeWhile (fun x -> x <> ""))
        let tileData = l |> List.skipWhile (fun x -> x <> "") 
        let tileData = 
            if tileData.Length > 1 then
                tileData |> List.skip 1
            else
                tileData

        parseTilesHelper m tileData

let findMatchingEdges idx (m : Map<int, Tile>) =
    let edgeList = m |> Map.find idx
    let tileList = m |> Map.toList
    tileList
    |> List.filter (fun (idx, tile) -> 
        tile.edges |> List.exists (fun x -> edgeList.edges |> List.contains x)
    )

let parseTiles (l : string list) =
    parseTilesHelper Map.empty l

let tiles =
    readLines "input.txt"
    |> Seq.toList
    |> parseTiles

let tileIds = tiles |> Map.toList |> List.map (fun (id, _) -> id)

tileIds
|> List.map (fun x -> (x, findMatchingEdges x tiles))
|> List.filter (fun (_, x) -> x.Length <= 3)
|> List.map (fun (x, _) -> int64 x)
|> List.reduce (*)
|> fun x -> printfn "%A" x; x