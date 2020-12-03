#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

type XIndex = {
    limit: int
    index: int
}
with
    static member init limit = {
        limit = limit
        index = 0
    }
    member this.increase number = 
        { this with 
            index = (this.index + number) % this.limit
        }
    member this.getIndex = this.index

type YIndex = {
    limit: int
    index: int
}
with
    static member init limit = {
        limit = limit
        index = 0
    }
    member this.increase number = 
        { this with 
            index = this.index + number
        }
    member this.isOverLimit =
        this.index >= this.limit
    member this.getIndex = this.index

type Position = {
    x: XIndex
    y: YIndex
    xConfigIncrease: int
    yConfigIncrease: int
}
with
    static member init x y xConfigIncrease yConfigIncrease =
        {
            x = XIndex.init x
            y = YIndex.init y
            xConfigIncrease = xConfigIncrease
            yConfigIncrease = yConfigIncrease
        }

    member this.getNext =
        { this with
            x = this.x.increase this.xConfigIncrease
            y = this.y.increase this.yConfigIncrease
        }
    member this.isOverLimit = this.y.isOverLimit

type Result = {
    position : Position
    numberOfTrees : int
}


let nextStep (result : Result) (map : char list list) =
    let newPosition = result.position.getNext
    if newPosition.isOverLimit then
        { result with
            position = newPosition
        }
    else
        {
            position = newPosition
            numberOfTrees = result.numberOfTrees + if map.[newPosition.y.getIndex].[newPosition.x.getIndex] = '#' then 1 else 0
        }


let lines = readLines "input.txt"
let width = lines |> Seq.head |> Seq.length
let height = lines |> Seq.length


let map = lines |> Seq.toList |> List.map (fun x -> Seq.toList x)

let calculate (xStep, yStep) = 
    let initResult = {
        position = Position.init width height xStep yStep
        numberOfTrees = 0
    }
    let mutable result = initResult

    while not result.position.isOverLimit do
        result <- nextStep result map
    result

let result =
    [(1, 1); (3, 1); (5, 1); (7, 1); (1, 2)]
    |> List.map calculate
    |> List.map (fun x -> x.numberOfTrees)
    |> List.reduce (*)

printfn "%A" result