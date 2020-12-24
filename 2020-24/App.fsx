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

let flipPlates (positions : HexCoordinate seq) =
    positions
    |> Seq.fold (fun flippedPlates curr ->
        match flippedPlates |> Map.tryFind curr with
        | Some PlateState.White | None -> flippedPlates.Add(curr, PlateState.Black)
        | Some PlateState.Black -> flippedPlates.Add(curr, PlateState.White)
    ) Map.empty

let countBlackPlates (plates : Map<HexCoordinate, PlateState>) =
    plates
    |> Map.toList
    |> List.sumBy (fun (_, color) -> 
        match color with
        | PlateState.Black -> 1
        | PlateState.White -> 0
    )

let str s = pstring s
let assumeOk p str =
    match run p str with
    | Success(result, _, _) -> result
    | Failure(errorMsg, _, _) -> failwith (sprintf "Could not parse correctly %s" errorMsg)

let stringDirections = (str "e" <|> str "se" <|> str "sw" <|> str "w" <|> str "nw" <|> str "ne")
let parser = (many stringDirections)

let readLines filePath = System.IO.File.ReadLines(filePath);

let lines =
    readLines "input.txt"
    |> Seq.map (assumeOk parser)
    |> Seq.map findPlate
    |> flipPlates
    |> countBlackPlates
    |> fun x -> printfn "%A" x; x