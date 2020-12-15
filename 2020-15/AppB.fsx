#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

type PreviousNumbers = {
    lastNumber: int64
    lastIndex: int64
    numbers: Map<int64, int64>
}

let getNextNumberState (numbers : PreviousNumbers) =
    match numbers.numbers.TryFind numbers.lastNumber with
    | Some lastNumberIndex ->
        {
            lastNumber = numbers.lastIndex - lastNumberIndex
            lastIndex = numbers.lastIndex + 1L
            numbers = numbers.numbers.Add(numbers.lastNumber, numbers.lastIndex)
        }
    | None ->
        {
            lastNumber = 0L
            lastIndex = numbers.lastIndex + 1L
            numbers = numbers.numbers.Add(numbers.lastNumber, numbers.lastIndex)

        }

let rec findNumberAtIndex (indexToFind : int64) (numbers : PreviousNumbers) =
    let nextNumber = getNextNumberState numbers
    if indexToFind = nextNumber.lastIndex then
        nextNumber
    else
        findNumberAtIndex indexToFind nextNumber

let listToMap l =
    let map = Map.empty
    l
    |> List.mapi (fun i x -> (int64 (i + 1), x))
    |> List.fold (fun (acc : Map<int64, int64>) (i, x) ->
        acc.Add(x, i)
    ) map

let listToPreviousNumbers l : PreviousNumbers  =
    let lastNumber = l |> List.last
    let previousNumbers = l |> List.take ((List.length l) - 1)
    {
        lastNumber = lastNumber
        lastIndex = l |> List.length |> int64
        numbers = listToMap previousNumbers
    }

let getSolution inputList index =
    inputList
    |> listToPreviousNumbers
    |> findNumberAtIndex index

printfn "%A" (getSolution [1L; 20L; 11L; 6L; 12L; 0L] 30000000L)
