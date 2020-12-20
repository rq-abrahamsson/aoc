open System

open FSharpPlus
open FParsec

let readLines filePath = System.IO.File.ReadLines(filePath);

let test p str =
    match run p str with
    | Success(result, _, _)   -> printfn "Success: %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

let getResult p str =
    match run p str with
    | Success(result, _, _)   -> Some result
    | Failure(errorMsg, _, _) -> None

let ws = spaces // skips any whitespace
let str_ws s = pstring s >>. ws
let number = pint64 .>> ws

let aExpression =
    let opp = new OperatorPrecedenceParser<int64,unit,unit>()
    let expr = opp.ExpressionParser
    opp.TermParser <- number <|> between (str_ws "(") (str_ws ")") expr
    opp.AddOperator(InfixOperator("+", ws, 1, Associativity.Left, (+)))
    opp.AddOperator(InfixOperator("*", ws, 1, Associativity.Left, (*)))
    ws >>. expr .>> eof

let bExpression =
    let opp = new OperatorPrecedenceParser<int64,unit,unit>()
    let expr = opp.ExpressionParser
    opp.TermParser <- number <|> between (str_ws "(") (str_ws ")") expr
    opp.AddOperator(InfixOperator("+", ws, 2, Associativity.Left, (+)))
    opp.AddOperator(InfixOperator("*", ws, 1, Associativity.Left, (*)))
    ws >>. expr .>> eof

[<EntryPoint>]
let main argv =
    let lines =
        readLines "input.txt"
        |> Seq.toList
    lines
    |> List.map (getResult aExpression)
    |> List.sumBy (function
        | Some v -> v
        | None -> 0L)
    |> fun x -> printfn "A result: %A" x; x
    lines
    |> List.map (getResult bExpression)
    |> List.sumBy (function
        | Some v -> v
        | None -> 0L)
    |> fun x -> printfn "B result: %A" x; x
    0