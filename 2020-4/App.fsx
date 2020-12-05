#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let lines = 
    readLines "input.txt"
    |> Seq.toList


//let tmp = lines |> List.findIndex (fun x -> x = "")
let passports =
    lines
    |> List.fold (fun (newList : string list, builtString : string) (elem : string) -> 
        if elem = "" then
            (builtString :: newList, "")
        else
            (newList, (sprintf "%s %s" builtString elem).Trim([|' '|])) 
    ) (List.empty, "")
    |> fun (passports, lastPassport) -> lastPassport :: passports


let getPassportFields (passport : string) =
    passport.Split(' ', ':')
    |> Array.toList

let countPassportFields (passportData : string list) =
    passportData
    |> List.filter (fun x ->
        x = "byr" ||
        x = "iyr" ||
        x = "eyr" ||
        x = "hgt" ||
        x = "hcl" ||
        x = "ecl" ||
        x = "pid"
    )
    |> List.length

let validatePassport fieldAmount =
    fieldAmount = 7

passports
|> List.map getPassportFields
|> List.map countPassportFields
|> List.filter validatePassport
|> List.length
|> printfn "%A"