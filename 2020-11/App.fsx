#r "nuget: FSharpPlus"
open FSharpPlus

type Coordinate = {
    x: int
    y: int
}

type Seatmap = char array array

let printSeatmap (seatmap : Seatmap) =
    printfn "-------------------"
    seatmap 
    |> Array.map (fun line -> line |> System.String |> printfn "%s")
    printfn "-------------------"
    seatmap

let readLines filePath = System.IO.File.ReadLines(filePath);

let createCoordinate x y : Coordinate = { x = x; y = y }

let changeX number (coordinate : Coordinate) = 
    {
        coordinate with
        x = coordinate.x + number
    }

let changeY number (coordinate : Coordinate) = 
    {
        coordinate with
        y = coordinate.y + number
    }

let increaseY (coordinate : Coordinate) = 
    changeY 1 coordinate

let decreaseY (coordinate : Coordinate) = 
    changeY -1 coordinate

let increaseX (coordinate : Coordinate) = 
    changeX 1 coordinate

let decreaseX (coordinate : Coordinate) = 
    changeX -1 coordinate

let getStateForCoordinate (seatmap : Seatmap) (position : Coordinate) = 
    if position.y < 0 || position.x < 0 || position.y >= Array.length seatmap || position.x >= Array.length seatmap.[0] then
        None
    else
        Some seatmap.[position.y].[position.x]

let countSurrounding (seatType : char) (seatmap : Seatmap) (position : Coordinate) =
    let positions = 
        [
            position |> increaseX
            position |> decreaseX
            position |> increaseY
            position |> decreaseY
            position |> increaseX |> increaseY
            position |> increaseX |> decreaseY
            position |> decreaseX |> increaseY
            position |> decreaseX |> decreaseY
        ]
    positions 
    |> List.map (getStateForCoordinate seatmap)
    |> List.sumBy 
        (function
        | Some t when t = seatType -> 1
        | _ -> 0)

let countSurroundingFree (seatmap : Seatmap) (position : Coordinate) =
    countSurrounding 'L' seatmap position

let countSurroundingBusy (seatmap : Seatmap) (position : Coordinate) =
    countSurrounding '#' seatmap position

let nextStateForCoordinate (seatmap : Seatmap) (position : Coordinate) =
    match getStateForCoordinate seatmap position with
    | Some 'L' -> 
        match countSurroundingBusy seatmap position with
        | 0 -> '#'
        | _ -> 'L'
    | Some '#' -> 
        match countSurroundingBusy seatmap position with
        | amount when amount >= 4 -> 'L'
        | _ -> '#'
    | Some '.' -> '.'
    | _ -> '.'

let nextStateForSeatmap (seatmap : Seatmap) =
    seatmap 
        |> Array.mapi (fun y line ->
            line |> Array.mapi (fun x element ->
                nextStateForCoordinate seatmap (createCoordinate x y)
            ))

let rec getStableSeatmap (seatmap : Seatmap) =
    let nextState = nextStateForSeatmap seatmap
    if nextState = seatmap then
        nextState
    else
        getStableSeatmap nextState

let countTotalOccupiedSeats (seatmap : Seatmap) =
    seatmap |> Array.sumBy (fun line -> line |> Array.sumBy (fun x -> if x = '#' then 1 else 0))

let seatmap = 
    readLines "input.txt"
    |> Seq.map String.toArray
    |> Seq.toArray

seatmap
|> printSeatmap
|> getStableSeatmap
|> printSeatmap
|> countTotalOccupiedSeats
|> fun x -> printfn "%A" x; x