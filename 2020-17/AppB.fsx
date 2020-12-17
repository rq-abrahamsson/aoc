#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

type Coordinate = {
    x: int
    y: int
    z: int
    w: int
}

type Game = Map<Coordinate, char>
type NeighborData = Map<Coordinate, int>

let increaseNeighborForCoordinate (map : NeighborData) x y z w =
    let coord = { x = x; y = y; z = z; w = w}
    let e = map.TryFind(coord)
    match e with
    | Some e ->
        map.Add(coord, e + 1)
    | None ->
        map.Add(coord, 1)


let getNeighborAmount (game : Game) =
    let mutable neighborAmount : NeighborData = Map.empty
    game 
    |> Map.toList
    |> List.iter (fun ({ x = x; y = y; z = z; w = w}, _) ->
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y - 1) (z - 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y - 1) (z - 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y - 1) (z - 1) (w - 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y + 1) (z - 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y + 1) (z - 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y + 1) (z - 1) (w - 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1)  y      (z - 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x       y      (z - 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1)  y      (z - 1) (w - 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y - 1)  z      (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y - 1)  z      (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y - 1)  z      (w - 1)
 
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y + 1)  z      (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y + 1)  z      (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y + 1)  z      (w - 1)
 
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1)  y       z      (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount x        y       z      (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1)  y       z      (w - 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y - 1) (z + 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y - 1) (z + 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y - 1) (z + 1) (w - 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y + 1) (z + 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y + 1) (z + 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y + 1) (z + 1) (w - 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1)  y      (z + 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x       y      (z + 1) (w - 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1)  y      (z + 1) (w - 1)


        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y - 1) (z - 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y - 1) (z - 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y - 1) (z - 1)  w     

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y + 1) (z - 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y + 1) (z - 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y + 1) (z - 1)  w     

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1)  y      (z - 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x       y      (z - 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1)  y      (z - 1)  w     

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y - 1)  z       w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y - 1)  z       w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y - 1)  z       w     
 
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y + 1)  z       w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y + 1)  z       w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y + 1)  z       w     
 
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1)  y       z       w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1)  y       z       w     

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y - 1) (z + 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y - 1) (z + 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y - 1) (z + 1)  w     

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y + 1) (z + 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y + 1) (z + 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y + 1) (z + 1)  w     

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1)  y      (z + 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x       y      (z + 1)  w     
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1)  y      (z + 1)  w     


        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y - 1) (z - 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y - 1) (z - 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y - 1) (z - 1) (w + 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y + 1) (z - 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y + 1) (z - 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y + 1) (z - 1) (w + 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1)  y      (z - 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x       y      (z - 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1)  y      (z - 1) (w + 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y - 1)  z      (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y - 1)  z      (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y - 1)  z      (w + 1)
 
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y + 1)  z      (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y + 1)  z      (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y + 1)  z      (w + 1)
 
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1)  y       z      (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount x        y       z      (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1)  y       z      (w + 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y - 1) (z + 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y - 1) (z + 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y - 1) (z + 1) (w + 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1) (y + 1) (z + 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x      (y + 1) (z + 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1) (y + 1) (z + 1) (w + 1)

        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x - 1)  y      (z + 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount  x       y      (z + 1) (w + 1)
        neighborAmount <- increaseNeighborForCoordinate neighborAmount (x + 1)  y      (z + 1) (w + 1)
    )
    neighborAmount

let generateNextState (game : Game) (neighborAmount : NeighborData) =
    let mutable newGame : Game = Map.empty
    neighborAmount
    |> Map.toList
    |> List.iter (fun (coord, amount) -> 
        match game.TryFind(coord) with
        | Some _ -> 
            if amount = 2 || amount = 3 then
                newGame <- newGame.Add(coord, '#')
            else
                ()
        | None ->
            if amount = 3 then
                newGame <- newGame.Add(coord, '#')
            else
                ()
    )
    newGame


let getNextStateOfGame (game : Game) =
    game
    |> getNeighborAmount
    |> generateNextState game


let mutable initStateBuilder = Map.empty

let lines = 
    readLines "input.txt"
    |> Seq.map Seq.toList
    |> Seq.toList
    |> List.iteri (fun x l ->
        l |> List.iteri (fun y value ->
            let index = 
                {
                    x = x
                    y = y
                    z = 0
                    w = 0
                }
            if value = '#' then
                initStateBuilder <- initStateBuilder.Add(index, value)
            else
                ()
        )
    )

initStateBuilder
|> getNextStateOfGame
|> getNextStateOfGame
|> getNextStateOfGame
|> getNextStateOfGame
|> getNextStateOfGame
|> getNextStateOfGame
|> Map.toList
|> List.length
|> fun x -> printfn "%A" x; x

