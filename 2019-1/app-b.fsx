open System.IO

let output = 
    File.ReadLines "./output-b.txt" 
    |> Seq.map (fun s1 -> s1 |> int)

let input = 
    File.ReadLines "./input-a-real.txt" 
    |> Seq.map (fun s1 -> s1 |> int)

let rec Solve input =
    let result = (input / 3) - 2
    if result < 0 then
        0
    else
        result + (Solve result)


let answers = input |> Seq.map(Solve)
let sum = answers |> Seq.sum
printfn "%A" sum


// let tuples = Seq.zip answers output
// (tuples |> Seq.toList) |> (printfn "%A")

// let diffs = tuples |> Seq.map (fun t -> fst(t) - snd(t)) |> (printfn "%A")

