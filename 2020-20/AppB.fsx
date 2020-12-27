#r "nuget: FSharpPlus"
#r "nuget: FsUnit"
open FSharpPlus
open System

open NUnit.Framework
open FsUnit

let readLines filePath = System.IO.File.ReadLines(filePath);

type EdgeData = {
    top: string
    bottom: string
    left: string
    right: string
}

type Tile = {
    data: char list list
    edges: string list
    edgeData: EdgeData
}

type Edge =
    | Top of string
    | Bottom of string
    | Left of string
    | Right of string
    | TopReverse of string
    | BottomReverse of string
    | LeftReverse of string
    | RightReverse of string

type Instructions =
    | RotateClockwise
    | FlipHorizontal
    | FlipVertical

let getTileIndex (s : string) =
    s
    |> String.split [" "; ":"]
    |> Seq.toList
    |> fun x -> int x.[1]

module List =
    let removeLast l =
        l
        |> List.rev
        |> List.tail
        |> List.rev

let trimTile (tileData : char list list) : char list list =
    tileData.Tail
    |> List.removeLast
    |> List.map (fun x -> x.Tail |> List.removeLast)

let getTopEdge (tileData : char list list) =
    tileData.[0] |> String.Concat

let getBottomEdge (tileData : char list list) =
    tileData.[(tileData |> List.length) - 1] |> String.Concat

let getLeftEdge (tileData : char list list) =
    seq { for i in 0 .. tileData.Length - 1 -> tileData.[i].[0] } |> String.Concat

let getRightEdge (tileData : char list list) =
    seq { for i in 0 .. tileData.Length - 1 -> tileData.[i].[tileData.[i].Length - 1] } |> String.Concat

let getEdgeData (tileData : char list list) =
    let top = getTopEdge tileData
    let bottom = getBottomEdge tileData
    let left = getLeftEdge tileData
    let right = getRightEdge tileData
    { top = top; bottom = bottom; left = left; right = right }

let parseTile (m : Map<int, Tile>) (l : string list) =
    let idx = l |> List.head |> getTileIndex
    let tileData = 
        l 
        |> List.tail 
        |> List.takeWhile (fun x -> x <> "")
        |> List.map Seq.toList
    let edgeData = getEdgeData tileData
    let edges = [ edgeData.top; edgeData.bottom; edgeData.left; edgeData.right ]
    m.Add(idx,
        {
            data = tileData
            edges = 
                List.concat [
                    edges
                    edges |> List.map String.rev
                ]
            edgeData = edgeData
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

let findMatchingEdge (edges1 : string list) (edges2 : string list) =
    Set.intersect (edges1 |> Set.ofList) (edges2 |> Set.ofList)
    |> Set.toList

let findEdgePlacement (edgeData : EdgeData) (edges : string list) =
    if edgeData.top = edges.[0] then
        Edge.Top edges.[0]
    elif edgeData.bottom = edges.[0] then
        Edge.Bottom edges.[0]
    elif edgeData.left = edges.[0] then
        Edge.Left edges.[0]
    elif edgeData.right = edges.[0] then
        Edge.Right edges.[0]
    elif edgeData.top |> String.rev = edges.[0] then
        Edge.TopReverse edges.[0]
    elif edgeData.bottom |> String.rev = edges.[0] then
        Edge.BottomReverse edges.[0]
    elif edgeData.left |> String.rev = edges.[0] then
        Edge.LeftReverse edges.[0]
    else
        Edge.RightReverse edges.[0]

let findNeighbourPlacement tiles neighbourMapping (originalId, original) =
    let (_, neighbourIds) = neighbourMapping |> List.find (fun (x, _) -> x = originalId)
    // let original = tiles |> Map.find id
    let neighbours = 
        neighbourIds 
        |> List.map (fun id -> (id, tiles |> Map.tryFind id))
        |> List.choose (fun (x, neighbour) ->
                match neighbour with
                | Some neighbour -> Some (x, neighbour)
                | None -> None
            )
    neighbours
    |> List.map (fun (id, neighbour) -> 
        let matchingEdge = findMatchingEdge original.edges neighbour.edges
        let originalEdgePlacement =
            findEdgePlacement original.edgeData matchingEdge
            // |> fun x -> printfn "%A" x; x
        let neighbourEdgePlacement =
            findEdgePlacement neighbour.edgeData matchingEdge
            // |> fun x -> printfn "%A" x; x
        (id, neighbour, originalEdgePlacement, neighbourEdgePlacement)
    )

let flipTileVertical (tile : Tile) =
    let tileData = tile.data |> List.rev
    {
        tile with
        data = tileData
        edgeData = getEdgeData tileData
    }
[['a'; 'b']; ['c'; 'd']] 
|> List.rev
|> should equal [['c'; 'd']; ['a'; 'b']]

let flipTileHorizontal (tile : Tile) =
    let tileData = tile.data |> List.map List.rev
    {
        tile with
        data = tileData
        edgeData = getEdgeData tileData
    }
[['a'; 'b']; ['c'; 'd']] 
|> List.map List.rev
|> should equal [['b'; 'a']; ['d'; 'c']] 

let rotateTileClockwise (tile : Tile) =
    let tileData = tile.data |> List.transpose |> List.map List.rev
    {
        tile with
        data = tileData
        edgeData = getEdgeData tileData
    }
[[1; 2]; [4; 3]] |> List.transpose |> List.map List.rev
|> should equal [[4; 1]; [3; 2]] 

let rotateTileCounterClockwise (tile : Tile) =
    let tileData = tile.data |> List.map List.rev |> List.transpose
    {
        tile with
        data = tileData
        edgeData = getEdgeData tileData
    }
[[1; 2]; [4; 3]] |> List.map List.rev |> List.transpose
|> should equal [[2; 3]; [1; 4]] 

let getEdgeToFind (id, tile, otherEdge, edgeToTransform) =
    let edgeToFind =
        match otherEdge with
        | Top v -> Bottom v
        | Bottom v -> Top v
        | Left v -> Right v
        | Right v -> Left v
        | TopReverse v -> BottomReverse v
        | BottomReverse v -> TopReverse v
        | LeftReverse v -> RightReverse v
        | RightReverse v -> LeftReverse v
    (id, tile, edgeToFind, edgeToTransform)

let getInstructions (id, tile, goalEdge, edgeToTransform) =
    (id, tile,
    match goalEdge with
    | Top v -> 
        match edgeToTransform with
        | Top v -> []
        | Bottom v -> [RotateClockwise; RotateClockwise]
        | Left v -> [RotateClockwise]
        | Right v -> [RotateClockwise; RotateClockwise; RotateClockwise]
        | TopReverse v -> [FlipHorizontal]
        | BottomReverse v -> [FlipHorizontal; RotateClockwise; RotateClockwise]
        | LeftReverse v -> [RotateClockwise; FlipHorizontal]
        | RightReverse v -> [RotateClockwise; RotateClockwise; RotateClockwise; FlipHorizontal]
    | Bottom v ->
        match edgeToTransform with
        | Top v -> [RotateClockwise; RotateClockwise]
        | Bottom v -> []
        | Left v -> [RotateClockwise; RotateClockwise; RotateClockwise]
        | Right v -> [RotateClockwise]
        | TopReverse v -> [FlipHorizontal; RotateClockwise; RotateClockwise]
        | BottomReverse v -> [FlipHorizontal]
        | LeftReverse v -> [RotateClockwise; RotateClockwise; RotateClockwise; FlipHorizontal]
        | RightReverse v -> [RotateClockwise; FlipHorizontal]
    | Left v ->
        match edgeToTransform with
        | Top v -> [RotateClockwise; RotateClockwise; RotateClockwise]
        | Bottom v -> [RotateClockwise]
        | Left v -> []
        | Right v -> [RotateClockwise; RotateClockwise]
        | TopReverse v -> [RotateClockwise; RotateClockwise; RotateClockwise; FlipVertical]
        | BottomReverse v -> [RotateClockwise; FlipVertical]
        | LeftReverse v -> [FlipVertical]
        | RightReverse v -> [FlipVertical; RotateClockwise; RotateClockwise]
    | Right v ->
        match edgeToTransform with
        | Top v -> [RotateClockwise]
        | Bottom v -> [RotateClockwise; RotateClockwise; RotateClockwise]
        | Left v -> [RotateClockwise; RotateClockwise]
        | Right v -> []
        | TopReverse v -> [RotateClockwise; FlipVertical]
        | BottomReverse v -> [RotateClockwise; RotateClockwise; RotateClockwise; FlipVertical]
        | LeftReverse v -> [FlipVertical; RotateClockwise; RotateClockwise]
        | RightReverse v -> [FlipVertical]
    | TopReverse v ->
        match edgeToTransform with
        | Top v -> [FlipHorizontal]
        | Bottom v -> [RotateClockwise; RotateClockwise; FlipHorizontal]
        | Left v -> [RotateClockwise; FlipHorizontal]
        | Right v -> [RotateClockwise; RotateClockwise; RotateClockwise; FlipHorizontal]
        | TopReverse v -> []
        | BottomReverse v -> [RotateClockwise; RotateClockwise]
        | LeftReverse v -> [RotateClockwise]
        | RightReverse v -> [RotateClockwise; RotateClockwise; RotateClockwise]
    | BottomReverse v ->
        match edgeToTransform with
        | Top v -> [RotateClockwise; RotateClockwise; FlipHorizontal]
        | Bottom v -> [FlipHorizontal]
        | Left v -> [RotateClockwise; RotateClockwise; RotateClockwise; FlipHorizontal]
        | Right v -> [RotateClockwise; FlipHorizontal]
        | TopReverse v -> [RotateClockwise; RotateClockwise]
        | BottomReverse v -> []
        | LeftReverse v -> [RotateClockwise; RotateClockwise; RotateClockwise]
        | RightReverse v -> [RotateClockwise]
    | LeftReverse v ->
        match edgeToTransform with
        | Top v -> [RotateClockwise; RotateClockwise; RotateClockwise; FlipVertical]
        | Bottom v -> [RotateClockwise; FlipVertical]
        | Left v -> [FlipVertical]
        | Right v -> [RotateClockwise; RotateClockwise; FlipVertical]
        | TopReverse v -> [RotateClockwise; RotateClockwise; RotateClockwise]
        | BottomReverse v -> [RotateClockwise]
        | LeftReverse v -> []
        | RightReverse v -> [RotateClockwise; RotateClockwise]
    | RightReverse v ->
        match edgeToTransform with
        | Top v -> [RotateClockwise; FlipVertical]
        | Bottom v -> [RotateClockwise; RotateClockwise; RotateClockwise; FlipVertical]
        | Left v -> [RotateClockwise; RotateClockwise; FlipVertical]
        | Right v -> [FlipVertical]
        | TopReverse v -> [RotateClockwise]
        | BottomReverse v -> [RotateClockwise; RotateClockwise; RotateClockwise]
        | LeftReverse v -> [RotateClockwise; RotateClockwise]
        | RightReverse v -> []
    )

let runInstructionsOnTile (id, tile, instructions) =
    (id,
    instructions
    |> List.fold (fun acc curr -> 
        match curr with
        | RotateClockwise -> rotateTileClockwise acc
        | FlipHorizontal -> flipTileHorizontal acc
        | FlipVertical -> flipTileVertical acc
    ) tile
    )

let placeNeighbours neighbourPlacement =
    neighbourPlacement 
    |> List.map getInstructions
    |> List.map runInstructionsOnTile

let tiles =
    readLines "input-test.txt"
    |> Seq.toList
    |> parseTiles
    // |> fun x -> printfn "%A" x; x

let tileIds = tiles |> Map.toList |> List.map (fun (id, _) -> id)

let neighbourMapping =
    tileIds
    |> List.map (fun x -> (x, findMatchingEdges x tiles))
    |> List.map (fun (x, y) -> (x, y |> List.map (fun (z, _) -> z) |> List.filter (fun z -> z <> x)))
    // |> fun x -> printfn "%A" x; x


let rec placeTiles ids (decidedTiles : Map<int, Tile>) (tilesWithoutDecided : Map<int, Tile>) = 
    if ids |> List.isEmpty then
        decidedTiles
    else
        let id = ids.Head
        let original = decidedTiles |> Map.find id
        let neighbours =
            findNeighbourPlacement tilesWithoutDecided neighbourMapping (id, original)
            |> List.map getEdgeToFind
            // |> fun x -> printfn "%A" x; x
            |> placeNeighbours
            // |> fun x -> printfn "%A" x; x
        let decidedTiles = 
            neighbours |> List.fold (fun (acc : Map<int, Tile>) (id, neighbour) -> acc.Add(id, neighbour)) decidedTiles
        let tilesWithoutDecided = 
            neighbours |> List.fold (fun (acc : Map<int, Tile>) (id, _) -> acc.Remove(id)) tilesWithoutDecided 
        let newIds = ids.Tail @ (neighbours |> List.map (fun (id, _) -> id))
        placeTiles newIds decidedTiles tilesWithoutDecided

let cornerId = 1951
let original = tiles |> Map.find cornerId |> fun x -> printfn "%A" x; x
placeTiles [cornerId] (Map.empty.Add(cornerId, original)) (tiles.Remove(1171))
// |> Map.map (fun id x -> x.data |> trimTile )
|> fun x -> printfn "%A" x; x