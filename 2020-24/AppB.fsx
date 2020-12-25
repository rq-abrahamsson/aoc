#r "nuget: FSharpPlus"
#r "nuget: FParsec-Big-Data-Edition"
open FSharpPlus
open FParsec

type HexCoordinate = {
    a: int
    r: int
    c: int
}

type PlateState =
    | Black
    | White

let makeMove (position : HexCoordinate) move =
    match move with
    | "e" -> { position with c = position.c + 1}
    | "w" -> { position with c = position.c - 1}
    | "se" ->
        {
            a = 1 - position.a
            r = position.r + position.a
            c = position.c + position.a
        }
    | "sw" ->
        {
            a = 1 - position.a
            r = position.r + position.a
            c = position.c - (1 - position.a)
        }
    | "ne" -> 
        {
            a = 1 - position.a
            r = position.r - (1 - position.a)
            c = position.c + position.a
        }
    | "nw" ->
        {
            a = 1 - position.a
            r = position.r - (1 - position.a)
            c = position.c - (1 - position.a)
        }

let findPlate moves =
    moves |> List.fold makeMove { a = 0; r = 0; c = 0 }

let flipPlates (plates : Map<HexCoordinate, PlateState>) (positions : HexCoordinate seq) =
    positions
    |> Seq.fold (fun flippedPlates curr ->
        match flippedPlates |> Map.tryFind curr with
        | Some PlateState.White | None -> flippedPlates.Add(curr, PlateState.Black)
        | Some PlateState.Black -> flippedPlates.Add(curr, PlateState.White)
    ) plates

let countBlackPlates (plates : Map<HexCoordinate, PlateState>) =
    plates
    |> Map.toList
    |> List.sumBy (fun (_, color) -> 
        match color with
        | PlateState.Black -> 1
        | PlateState.White -> 0
    )
let printAmountOfBlackPlates (plates : Map<HexCoordinate, PlateState>) =
    // plates
    // |> Map.toList
    // |> List.iter (fun x -> printfn "%A" x)
    printfn "%A" (countBlackPlates plates)
    plates

let getNeighbourCoords position = 
    [
        { position with c = position.c + 1}
        { position with c = position.c - 1}
        {
            a = 1 - position.a
            r = position.r + position.a
            c = position.c + position.a
        }
        {
            a = 1 - position.a
            r = position.r + position.a
            c = position.c - (1 - position.a)
        }
        {
            a = 1 - position.a
            r = position.r - (1 - position.a)
            c = position.c + position.a
        }
        {
            a = 1 - position.a
            r = position.r - (1 - position.a)
            c = position.c - (1 - position.a)
        }
    ]

let rec countBlackPlatesNeighbourHelper (neighbourCount : Map<HexCoordinate, int>) (plates : HexCoordinate list) =
    if plates |> List.isEmpty then
        neighbourCount
    else
        let neighbours =
            getNeighbourCoords plates.Head
            |> List.fold (
                fun acc curr -> 
                    acc |> Map.change curr (
                        function
                        | Some v -> Some (v + 1)
                        | None -> Some 1
                    )
                ) neighbourCount
        countBlackPlatesNeighbourHelper neighbours plates.Tail


let countBlackPlateNeighbour (plates : Map<HexCoordinate, PlateState>) =
    plates
    |> Map.toSeq
    |> Seq.filter (fun (_, state) -> state  = PlateState.Black)
    |> Seq.map (fun (coord, _) -> coord)
    |> Seq.toList
    |> countBlackPlatesNeighbourHelper Map.empty

let getNextDay (plates : Map<HexCoordinate, PlateState>) =
    let neighbours = countBlackPlateNeighbour plates
    let firstCoordsToFlip =
        neighbours 
        |> Map.toList
        |> List.filter (fun (k, v) -> 
            match plates.TryFind k with
            | Some plateValue -> 
                if plateValue = PlateState.Black && (v > 2 || v = 0) then
                    true
                elif plateValue = PlateState.White && v = 2 then
                    true
                else
                    false
            | None -> 
                if v = 2 then
                    true
                else
                    false
        )
        |> List.map (fun (c, _) -> c)
    let moreCoordsToFlip =
        plates
        |> Map.toList
        |> List.filter (fun (k, v) -> 
            match neighbours.TryFind k with
            | Some nValue -> 
                if v = PlateState.Black && (nValue > 2 || nValue = 0) then
                    true
                elif v = PlateState.White && nValue = 2 then
                    true
                else
                    false
            | None -> 
                if v = PlateState.Black then
                    true
                else
                    false
        )
        |> List.map (fun (c, _) -> c)
    let coordsToFlip =
        List.concat [firstCoordsToFlip; moreCoordsToFlip]
        |> List.distinct
        |> Seq.ofList
    flipPlates plates coordsToFlip

let runDays amount (plates : Map<HexCoordinate, PlateState>) =
    [1..amount]
    |> List.fold (fun acc _ -> getNextDay acc) plates


let str s = pstring s
let assumeOk p str =
    match run p str with
    | Success(result, _, _) -> result
    | Failure(errorMsg, _, _) -> failwith (sprintf "Could not parse correctly %s" errorMsg)

let stringDirections = (str "e" <|> str "se" <|> str "sw" <|> str "w" <|> str "nw" <|> str "ne")
let parser = (many stringDirections)

let readLines filePath = System.IO.File.ReadLines(filePath);

let startPlates =
    readLines "input.txt"
    |> Seq.map (assumeOk parser)
    |> Seq.map findPlate
    |> (flipPlates Map.empty)
    |> runDays 100
    |> printAmountOfBlackPlates