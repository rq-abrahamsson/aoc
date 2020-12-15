#r "nuget: FSharpPlus"
open FSharpPlus
open System

let readLines filePath = System.IO.File.ReadLines(filePath);

type Instruction =
    | Mask of string
    | Mem of (int * string)

type ProgramState =
    {
        memory: Map<int, string>
        currentMask: string
        instructions: Instruction list
    }

let zeroString = "000000000000000000000000000000000000"

let parseInstruction str =
    str
    |> String.split [" "]
    |> List.ofSeq
    |> fun x -> (x.[0], x.[2])
    |> (fun (instruction, value) ->
        if instruction = "mask" then
            Mask value
        else
            let idx = 
                instruction
                |> String.split ["["]
                |> List.ofSeq
                |> fun x -> x.[1]
                |> String.trimEnd [']']
                |> int
            let v =
                let binaryString = Convert.ToString(int value, 2)
                let length = (String.length zeroString) - (String.length binaryString)
                zeroString 
                |> fun x -> sprintf "%s%s" (x |> String.take length) (Convert.ToString(int value, 2))
            Mem (idx, v)
    )

let maskValue mask value =
    Seq.map2 (fun m v -> 
        if m = 'X' then
            v
        else 
            m
    ) mask value 
    |> Seq.map string
    |> String.concat ""

let nextProgramState (program : ProgramState) =
    let nextInstruction::restInstructions = program.instructions
    let program = {
        program with
        instructions = restInstructions
    }
    match nextInstruction with
    | Mask data -> 
        { program with
            currentMask = data
        }
    | Mem (index, value) -> 
        { program with
            memory = program.memory.Add(index, maskValue program.currentMask value)
        }


let rec runProgram (program : ProgramState) =
    let nextState = nextProgramState program
    if nextState.instructions |> List.isEmpty then
        nextState
    else
        runProgram nextState

let memoryMapToResult (programState : ProgramState) =
    programState.memory
    |> Map.toList
    |> List.map (fun (x, y) -> Convert.ToInt64(y, 2))
    |> List.sum

let instructions = 
    readLines "input.txt"
    |> Seq.map parseInstruction
    |> Seq.toList

let programStartState = {
    memory = Map.empty
    currentMask = zeroString
    instructions = instructions 
}

runProgram programStartState
|> memoryMapToResult
|> printfn "%A"
