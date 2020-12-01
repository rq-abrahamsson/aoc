#r "nuget: FSharpPlus"
open FSharpPlus

let x = String.replace "old" "new" "Good old days"
printfn "%A" x