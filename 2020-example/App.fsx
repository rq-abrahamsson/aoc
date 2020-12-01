#r "nuget: FSharpPlus"
open FSharpPlus

let x = String.replace "old" "new" "Good old days"
printfn "%A" x

let readLines filePath = System.IO.File.ReadLines(filePath);

let lines = readLines "input.txt"
printfn "%A" lines

let lineList = lines |> Seq.toList
printfn "%A" lineList