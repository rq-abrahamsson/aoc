#r "nuget: FsUnit"
open NUnit.Framework
open FsUnit


let readLines filePath = System.IO.File.ReadLines(filePath);

let rec combinations (head : int) (tail : int list) =
    if tail |> List.isEmpty then
        []
    else if tail.Head - head <= 3 then
        tail.Head :: combinations head tail.Tail
    else
        []

let rec getPossibleSteps (s : int list) =
    let head :: tail = s
    let tail = tail |> List.toSeq
    if s |> List.isEmpty || tail |> Seq.isEmpty then
        1
    else 
        tail
        |> Seq.takeWhile (fun x -> x <= head + 3)
        |> Seq.map (fun x -> (x, tail |> Seq.skipWhile (fun y -> y < x)))
        |> Seq.map (fun (_, x) -> x |> Seq.toList)
        |> Seq.map getPossibleSteps
        |> Seq.sum
        // tail 
        // |> Seq.takeWhile (fun x -> x <= head + 3)
        // |> fun x -> x |> Seq.toList |> combinations head |> printfn "combinations %A"; x
        // |> fun x -> (x |> Seq.length, tail |> Seq.skip ((Seq.length x) - 1))
        // |> fun (x, l) -> (x, l |> List.ofSeq)
        // |> fun (amount, l) -> 
        //         printfn "%A * %A" amount (getPossibleSteps l)
        //         getPossibleSteps l

// let result = 
//     readLines "input-test.txt"
//     |> Seq.map int
//     |> Seq.sort
//     |> List.ofSeq
//     |> (fun x -> 0 :: (x @ [x.[List.length x - 1] + 3]))
//     |> fun x -> printfn "%A" x; x
//     |> getPossibleSteps


let runTest input output =
    input
    |> getPossibleSteps
    |> fun x -> printfn "%A" x; x
    |> should equal (output |> List.length)

// TEST 1
let inputArray = [40; 43; 46; 47; 48; 49; 52]
let possibleOutputs = 
    [
        [40; 43; 46; 47; 48; 49; 52]
        [40; 43; 46; 48; 49; 52]
        [40; 43; 46; 47; 49; 52]
        [40; 43; 46; 49; 52]
    ]
runTest inputArray possibleOutputs
 
// TEST 2
let inputArray2 = [1; 4; 5; 6; 7; 10; 11; 12; 15; 16; 19]
let possibleOutputs2 = 
    [
        [0; 1; 4; 5; 6; 7; 10; 11; 12; 15; 16; 19; 22]
        [0; 1; 4; 5; 6; 7; 10; 12; 15; 16; 19; 22]
        [0; 1; 4; 5; 7; 10; 11; 12; 15; 16; 19; 22]
        [0; 1; 4; 5; 7; 10; 12; 15; 16; 19; 22]
        [0; 1; 4; 6; 7; 10; 11; 12; 15; 16; 19; 22]
        [0; 1; 4; 6; 7; 10; 12; 15; 16; 19; 22]
        [0; 1; 4; 7; 10; 11; 12; 15; 16; 19; 22]
        [0; 1; 4; 7; 10; 12; 15; 16; 19; 22]
    ]
runTest inputArray2 possibleOutputs2

// TEST 3
let inputArray3 = [1; 3; 4; 5]
let possibleOutputs3 = 
    [
        [0; 1; 3; 4; 5]
        [0; 1; 3; 5]
        [0; 1; 4; 5]
    ]
runTest inputArray3 possibleOutputs3

// TEST 4
let inputArray4 = [1; 3; 4; 5; 6]
let possibleOutputs4 = 
    [
        [0; 1; 3; 4; 5; 6]
        [0; 1; 3; 4; 6]
        [0; 1; 3; 5; 6]
        [0; 1; 3; 6]
        [0; 1; 4; 5; 6]
        [0; 1; 4; 6]
    ]
runTest inputArray4 possibleOutputs4

// TEST 5
let inputArray5 = [10; 11; 12; 15]
let possibleOutputs5 = 
    [
        [10; 11; 12; 15]
        [10; 12; 15]
    ]
runTest inputArray5 possibleOutputs5
