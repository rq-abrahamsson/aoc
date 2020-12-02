#r "nuget: FSharpPlus"
open FSharpPlus

type PasswordData = {
    lowerP: int
    upperP: int
    letter: char
    password: string
}
let parsePasswordData (line : string) : PasswordData =
    let eh = line.Split(' ', '-', ':')
    {
        lowerP = int eh.[0]
        upperP = int eh.[1]
        letter = char eh.[2]
        password = eh.[4]
    }
let countPasswordAmount (passwordData : PasswordData) = // (password : string) (p1: int) (p2 : int) (letter : char) =
    let pList =
        passwordData.password 
        |> Seq.toList 
    let count = if pList.[passwordData.lowerP - 1] = passwordData.letter then 1 else 0
    let count = if pList.[passwordData.upperP - 1] = passwordData.letter then count + 1 else count
    count

let passwordOk (passwordData : PasswordData) =
    let count = countPasswordAmount passwordData
    count = 1

let readLines filePath = System.IO.File.ReadLines(filePath);

let lines = readLines "input.txt"
let result = 
    lines 
    |> Seq.map parsePasswordData
    |> Seq.map passwordOk
    |> Seq.filter (fun x -> x)
    |> Seq.length

printfn "%A" result