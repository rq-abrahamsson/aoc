#r "nuget: FSharpPlus"
open FSharpPlus

type PasswordData = {
    lower: int
    upper: int
    letter: char
    password: string
}
let parsePasswordData (line : string) : PasswordData =
    let eh = line.Split(' ', '-', ':')
    {
        lower = int eh.[0]
        upper = int eh.[1]
        letter = char eh.[2]
        password = eh.[4]
    }
let countPasswordAmount (password : string) (letter : char) =
    password 
    |> Seq.toList 
    |> List.filter (fun x -> x = letter) 
    |> List.length

let passwordOk (passwordData : PasswordData) =
    let count = countPasswordAmount passwordData.password passwordData.letter
    count <= passwordData.upper && count >= passwordData.lower

let readLines filePath = System.IO.File.ReadLines(filePath);

let lines = readLines "input.txt"
let result = 
    lines 
    |> Seq.map parsePasswordData
    |> Seq.map passwordOk
    |> Seq.filter (fun x -> x)
    |> Seq.length

printfn "%A" result