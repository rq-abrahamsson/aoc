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

let rec getStateInDirection (directionIncreaseFunction : Coordinate -> Coordinate) (seatmap : Seatmap) (position : Coordinate) =
    let newPosition = directionIncreaseFunction position
    match getStateForCoordinate seatmap newPosition with
    | Some '.' -> getStateInDirection directionIncreaseFunction seatmap newPosition
    | Some state -> Some state
    | None -> None

let countSurrounding (seatType : char) (seatmap : Seatmap) (position : Coordinate) =
    let positions = 
        [
            getStateInDirection increaseX seatmap position
            getStateInDirection decreaseX seatmap position
            getStateInDirection increaseY seatmap position
            getStateInDirection decreaseY seatmap position
            getStateInDirection (increaseX >> increaseY) seatmap position
            getStateInDirection (increaseX >> decreaseY) seatmap position
            getStateInDirection (decreaseX >> increaseY) seatmap position
            getStateInDirection (decreaseX >> decreaseY) seatmap position
        ]
    positions 
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
        | amount when amount >= 5 -> 'L'
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


// let seatmap2 =
//     seatmap 
//     |> printSeatmap
//     |> nextStateForSeatmap 
//     |> printSeatmap

// countSurrounding '#' seatmap2 (createCoordinate 0 0)
// |> printfn "%A"

// seatmap
// |> printSeatmap
// |> nextStateForSeatmap 
// |> printSeatmap
// |> nextStateForSeatmap 
// |> printSeatmap
// |> countTotalOccupiedSeats
// |> fun x -> printfn "%A" x; x

seatmap
|> printSeatmap
|> getStableSeatmap
|> printSeatmap
|> countTotalOccupiedSeats
|> fun x -> printfn "%A" x; x