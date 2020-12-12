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
    position: Coordinate
    waypoint: Coordinate
    instructions: Instruction list
}

let parseInstruction (str : string) =
    (str.Substring(0, 1), int <| str.Substring(1))

let updatePosition (shipState : ShipState) (position : Coordinate) =
    {
        shipState with
        position = position
    }

let updateWaypoint (shipState : ShipState) (waypoint : Coordinate) =
    {
        shipState with
        waypoint = waypoint 
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
let moveWaypoint (direction : Direction) (value : int) =
    match direction with
    | North -> changeY value 
    | South -> changeY -value 
    | West -> changeX -value 
    | East -> changeX value 

let moveInWaypointDirection (waypoint : Coordinate) (value : int) (position : Coordinate) =
    {
        x = position.x + (waypoint.x * value)
        y = position.y + (waypoint.y * value)
    }

let rotateOneStepLeft (waypoint : Coordinate) =
    {
        x = -waypoint.y
        y = waypoint.x
    }

let rotateOneStepRight (waypoint : Coordinate) =
    {
        x = waypoint.y
        y = -waypoint.x
    }

let turnWaypoint (waypoint : Coordinate) (rotation: Rotation) : Coordinate =
    match rotation with
    | Left steps -> 
        [0..steps-1] |> List.fold (fun acc _curr -> rotateOneStepLeft acc) waypoint
    | Right steps ->
        [0..steps-1] |> List.fold (fun acc _curr -> rotateOneStepRight acc) waypoint

let readInstruction (state : ShipState) : (Instruction * ShipState) =
    let (instruction :: tail) = state.instructions
    (instruction, { state with instructions = tail })

let getNextState (state : ShipState) : ShipState =
    let (instruction, state) = readInstruction state
    match instruction with
    | ("N", value) -> moveWaypoint North value state.waypoint |> updateWaypoint state
    | ("S", value) -> moveWaypoint South value state.waypoint |> updateWaypoint state
    | ("W", value) -> moveWaypoint West value state.waypoint |> updateWaypoint state
    | ("E", value) -> moveWaypoint East value state.waypoint |> updateWaypoint state
    | ("F", value) -> moveInWaypointDirection state.waypoint value state.position |> updatePosition state
    | ("R", value) -> turnWaypoint state.waypoint (Right (value / 90)) |> updateWaypoint state
    | ("L", value) -> turnWaypoint state.waypoint (Left (value / 90)) |> updateWaypoint state

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
    position = createCoordinate 0 0
    waypoint = createCoordinate 10 1
    instructions = instructions
}

startState
|> runInstructions
|> getManhattanDistance
|> fun x -> printfn "%A" x; x