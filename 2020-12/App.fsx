#r "nuget: FSharpPlus"
open System

let readLines filePath = System.IO.File.ReadLines(filePath);

type Coordinate = {
    x: int
    y: int
}

type Direction =
    | North
    | East
    | West
    | South

type Rotation =
    | Left of int
    | Right of int

type Instruction = (string * int)

type ShipState = {
    faceDirection: Direction
    position: Coordinate
    instructions: Instruction list
}

let parseInstruction (str : string) =
    (str.Substring(0, 1), int <| str.Substring(1))

let updatePosition (shipState : ShipState) (position : Coordinate) =
    {
        shipState with
        position = position
    }

let updateDirection (shipState : ShipState) (direction : Direction) =
    {
        shipState with
        faceDirection = direction
    }

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
let move (direction : Direction) (value : int) =
    match direction with
    | North -> changeY value 
    | South -> changeY -value 
    | West -> changeX -value 
    | East -> changeX value 

let rotateOneStepLeft direction =
    match direction with
    | East -> North
    | North -> West
    | West -> South
    | South -> East

let rotateOneStepRight direction =
    match direction with
    | North -> East
    | East -> South
    | South -> West
    | West -> North

let turn (faceDirection : Direction) (rotation: Rotation) : Direction =
    match rotation with
    | Left steps -> 
        [0..steps-1] |> List.fold (fun acc _curr -> rotateOneStepLeft acc) faceDirection
    | Right steps ->
        [0..steps-1] |> List.fold (fun acc _curr -> rotateOneStepRight acc) faceDirection

let readInstruction (state : ShipState) : (Instruction * ShipState) =
    let (instruction :: tail) = state.instructions
    (instruction, { state with instructions = tail })

let getNextState (state : ShipState) : ShipState =
    let (instruction, state) = readInstruction state
    match instruction with
    | ("N", value) -> move North value state.position |> updatePosition state
    | ("S", value) -> move South value state.position |> updatePosition state
    | ("W", value) -> move West value state.position |> updatePosition state
    | ("E", value) -> move East value state.position |> updatePosition state
    | ("F", value) -> move state.faceDirection value state.position |> updatePosition state
    | ("R", value) -> turn state.faceDirection (Right (value / 90)) |> updateDirection state
    | ("L", value) -> turn state.faceDirection (Left (value / 90)) |> updateDirection state

let rec runInstructions (state: ShipState) =
    let nextState = getNextState state
    if nextState.instructions |> List.isEmpty then
        nextState
    else
        runInstructions nextState
let getManhattanDistance (state : ShipState) =
    Math.Abs(state.position.x) + Math.Abs(state.position.y)

let instructions = 
    readLines "input.txt"
    |> Seq.map parseInstruction
    |> Seq.toList

let startState = {
    faceDirection = Direction.East
    position = createCoordinate 0 0
    instructions = instructions
}

startState
|> runInstructions
|> getManhattanDistance
|> fun x -> printfn "%A" x; x