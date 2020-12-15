#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let getNextNumber (numbers : int list) =
    let lastNumber = numbers |> List.last
    let previousNumbers = numbers |> List.take ((List.length numbers) - 1)
    let numberFoundIndex = previousNumbers |> List.tryFindIndexBack (fun x -> x = lastNumber)
    match numberFoundIndex with
    | Some lastNumberIndex ->
        ((List.length numbers) - 1) - lastNumberIndex
    | None ->
        0

let rec findNumberAtIndex (indexToFind : int) (numbers : int list) =
    let nextNumber = getNextNumber numbers
    let newList = numbers @ [nextNumber]
    if indexToFind = (newList |> List.length) then
        newList
    else
        findNumberAtIndex indexToFind newList



let lines = 
    readLines "input.txt"
    |> List.ofSeq
    |> List.map int
    // |> getNextNumber
    |> findNumberAtIndex 2020
    |> List.last
    |> fun x -> printfn "%A" x; x