#r "nuget: FSharpPlus"
open FSharpPlus
open System
open System.Text.RegularExpressions

let readLines filePath = System.IO.File.ReadLines(filePath);

let lines = 
    readLines "input.txt"
    |> Seq.toList


let passports =
    lines
    |> List.fold (fun (newList : string list, builtString : string) (elem : string) -> 
        if elem = "" then
            (builtString :: newList, "")
        else
            (newList, (sprintf "%s %s" builtString elem).Trim([|' '|])) 
    ) (List.empty, "")
    |> fun (passports, lastPassport) -> lastPassport :: passports


let getPassportFieldsWithData (passport : string) =
    passport.Split(' ')
    |> Array.toList
    |> List.map (fun x -> 
        let s = x.Split(':')
        (s.[0], s.[1])
        )

let validateCorrectPassportFieldsExist (passportData : (string * string) list) =
    passportData
    |> List.filter (fun (x, _) ->
        x = "byr" ||
        x = "iyr" ||
        x = "eyr" ||
        x = "hgt" ||
        x = "hcl" ||
        x = "ecl" ||
        x = "pid"
    )
    |> List.length
    |> fun x -> x = 7

let validateCorrectPassportFieldContent (passportData : (string * string) list) =
    passportData
    |> List.filter 
        (function
        | ("byr", data) -> int data >= 1920 && int data <= 2002
        | ("iyr", data) -> int data >= 2010 && int data <= 2020
        | ("eyr", data) -> int data >= 2020 && int data <= 2030
        | ("hgt", data) ->
            let len = data.Substring(0, data.Length - 2)
            let unit = data.Substring(data.Length - 2)
            (unit = "cm" && int len >= 150 && int len <= 193) ||
                (unit = "in" && int len >= 59 && int len <= 76)
        | ("hcl", data) -> 
            let color = data.Substring(1)
            let regex = Regex(@"\b([a-f]|[0-9])+\b", RegexOptions.Compiled)
            data.[0] = '#' && color.Length = 6 && regex.Matches(color).Count > 0
        | ("ecl", data) ->
            match data with
            | "amb" | "blu" | "brn" | "gry" | "grn" | "hzl" | "oth" -> true
            | _ -> false
        | ("pid", data) -> 
            let couldParse, _ = Int32.TryParse data
            data |> String.length = 9 && couldParse
        | _ -> false
        )
    |> List.length
    |> fun x -> x = 7


passports
|> List.map getPassportFieldsWithData
|> List.filter validateCorrectPassportFieldsExist
|> List.filter validateCorrectPassportFieldContent 
|> List.length
|> printfn "%A"