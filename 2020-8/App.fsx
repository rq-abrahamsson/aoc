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
            row = index
        }
    )

let runProgram (program : Instruction list) =
    let mutable state : ProgramState = {
        accumulator = 0
        currentRow = 1
        visitedRows = []
    }
    while not (state.visitedRows |> List.contains state.currentRow ) do
        state <- readProgramRow state program.[state.currentRow - 1]
    state

let program =
    readLines "input.txt"
    |> Seq.mapi getInstruction
    |> Seq.toList

runProgram program
|> printfn "%A"
