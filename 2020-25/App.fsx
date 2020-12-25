#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

let subjectNumber = 7L
let startValue = 1L
let m = 20201227L

let rec findLoopSizeHelper v count publicKey = 
    let newValue = ((v * subjectNumber) % m)
    if newValue = publicKey then
        count
    else
        findLoopSizeHelper newValue (count + 1L) publicKey

let findLoopSize = findLoopSizeHelper startValue 1L

let rec transformToEncryptionKeyHelper loopSize count sn publicKey = 
    let encryptionKey = ((publicKey * sn) % m)
    if loopSize - 1L = count then
        encryptionKey
    else
        transformToEncryptionKeyHelper loopSize (count + 1L) sn encryptionKey

let transformToEncryptionKey loopSize publicKey = 
    transformToEncryptionKeyHelper loopSize 1L publicKey publicKey

let tmp (list : (int64 * int64) list) =
    let first = list.Head
    let second = list.Tail.Head
    [
        (fst(first), snd(second))
        (fst(second), snd(first))
    ]


let lines = 
    readLines "input.txt"
    |> Seq.toList
    |> List.map int64
    |> List.map (fun publicKey -> (findLoopSize publicKey, publicKey))
    |> tmp
    |> List.map (fun (loopSize, publicKey) -> transformToEncryptionKey loopSize publicKey)
    |> fun x -> printfn "%A" x; x