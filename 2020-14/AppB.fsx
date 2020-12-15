#r "nuget: FSharpPlus"
open FSharpPlus
open System

let readLines filePath = System.IO.File.ReadLines(filePath);

type Instruction =
    | Mask of string
    | Mem of (int64 * string)

type ProgramState =
    {
        memory: Map<int64, string>
        currentMask: string
        instructions: Instruction list
    }

let zeroString = "000000000000000000000000000000000000"

let getAddressStringFromInt (number : int64) =
    let binaryString = Convert.ToString(number, 2)
    let length = (String.length zeroString) - (String.length binaryString)
    sprintf "%s%s" (zeroString |> String.take length) (Convert.ToString(number, 2))

let intFromAddressString str =
    Convert.ToInt64(str, 2)

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
                |> int64
            let v = getAddressStringFromInt (int64 value)
            Mem (idx, v)
    )

let maskValue mask value =
    Seq.map2 (fun (m : char) (v : char) ->
        if m = '0' then
            v
        else
            m
    ) mask value
    |> Seq.map string
    |> String.concat ""

let rec getAllBinaryCombinations number =
    if number = 1 then
        [[0]; [1]]
    else if number = 2 then
        List.allPairs [0;1] [0;1]
        |> List.map (fun (t1, t2) -> [t1; t2])
    else
        List.allPairs [0;1] (getAllBinaryCombinations (number - 1))
        |> List.map (fun (x, array) -> x :: array)

let floatingToIndexArray (indexWithFloating : string) =
    indexWithFloating
    |> Seq.sumBy (fun x -> if x = 'X' then 1 else 0)
    |> getAllBinaryCombinations
    |> List.map (fun binaryCombination ->
        let mutable i = 0
        indexWithFloating
        |> Seq.map (fun y ->
            if y = 'X' then
                i <- i + 1
                char <| binaryCombination.[i - 1].ToString()
            else
                y
        )
    )
    |> List.map (fun x -> x |> String.ofSeq)
    |> List.map intFromAddressString


let addValueToIndexes (map : Map<int64, string>) indexes value =
    indexes
    |> List.fold (fun (acc : Map<int64, string>) current ->
        acc.Add(current, value)
    ) map

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
        let indexWithFloating = maskValue program.currentMask (getAddressStringFromInt index)
        let indexList = floatingToIndexArray indexWithFloating
        { program with
            memory = addValueToIndexes program.memory indexList value
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
