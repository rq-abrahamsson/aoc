#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

type Instruction = 
    {
        operation: string
        argument: int
        row : int
    }

type ProgramState = 
    {
        accumulator: int
        currentRow: int
        visitedRows: int list

    }

let readProgramRow (state : ProgramState) (instruction : Instruction) =
    match instruction.operation with
    | "nop" -> 
        {
            accumulator = state.accumulator
            currentRow = state.currentRow + 1
            visitedRows = instruction.row :: state.visitedRows
        }
    | "acc" -> 
        {
            accumulator = state.accumulator + instruction.argument
            currentRow = state.currentRow + 1
            visitedRows = instruction.row :: state.visitedRows
        }
    | "jmp" -> 
        {
            accumulator = state.accumulator
            currentRow = state.currentRow + instruction.argument
            visitedRows = instruction.row :: state.visitedRows
        }

let getInstruction (index : int) (row : string) = 
    row
    |> String.split [ " " ]
    |> List.ofSeq
    |> (fun [op; arg] -> 
        {
            operation = op
            argument = int arg
            row = index + 1
        }
    )

let runProgram (program : Instruction list) =
    let mutable state : ProgramState = {
        accumulator = 0
        currentRow = 1
        visitedRows = []
    }
    while (not (state.visitedRows |> List.contains state.currentRow )) 
        && (state.currentRow - 1 < List.length program) do
        state <- readProgramRow state program.[state.currentRow - 1]
    if (state.currentRow - 1 >= List.length program) then
        (true, state.accumulator)
    else 
        (false, 0)

let modifyProgram (program : Instruction list) line =
    let mutable p = 
        program 
        |> List.toArray
    if program.[line - 1].operation = "nop" then
        p.[line - 1] <- {
            p.[line - 1] with
            operation = "jmp"
        }
    else if program.[line - 1].operation = "jmp" then
        p.[line - 1] <- {
            p.[line - 1] with
            operation = "nop"
        }
    p |> Array.toList

let program =
    readLines "input.txt"
    |> Seq.mapi getInstruction
    |> Seq.toList


let programs =
    [1..(List.length program)]
    |> List.map (modifyProgram program)
    |> List.map runProgram
    |> List.filter (fun (x, _) -> x)
// runProgram program |> printfn "%A"

programs
|> printfn "%A"
