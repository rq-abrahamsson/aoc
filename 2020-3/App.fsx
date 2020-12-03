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
}
with
    member this.getNext =
        {
            x = this.x.increase 3
            y = this.y.increase 1
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

let position = {
    x = XIndex.init width
    y = YIndex.init height
}
let initResult = {
    position = position
    numberOfTrees = 0
}
let mutable result = initResult

while not result.position.isOverLimit do
    result <- nextStep result map

printfn "%A" result.numberOfTrees