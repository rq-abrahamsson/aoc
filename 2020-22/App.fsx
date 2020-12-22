#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

type PlayerHand = int list

type GameStatus =
    | Playing
    | Winner of int

type Game = {
    cards: (PlayerHand * PlayerHand)
    status: GameStatus
}

let parsePlayer list =
    list
    |> List.skip 1
    |> List.takeWhile (fun x -> x <> "")
    |> List.map int

let parsePlayers list : Game =
    let player1 =
        parsePlayer list
    let player2 =
        list
        |> List.skipWhile (fun x -> x <> "")
        |> List.skip 1
        |> parsePlayer
    
    {
        cards = (player1, player2)
        status = Playing
    }

let updateHands winner ((player1, player2) : (PlayerHand * PlayerHand)) =
    if winner = 1 then
        (
            player1.Tail @ [player1.Head; player2.Head],
            player2.Tail
        )
    else
        (
            player1.Tail,
            player2.Tail @ [player2.Head; player1.Head]
        )
    

type RunGame = (PlayerHand * PlayerHand) list -> Game -> Game

let playRound (runGameFn : RunGame) ((player1, player2) : (PlayerHand * PlayerHand)) =
    let p1Card = player1.Head
    let p2Card = player2.Head
    if p1Card <= player1.Tail.Length && p2Card <= player2.Tail.Length then
        // printfn "Starting sub game"
        match runGameFn [] { cards = (player1.Tail |> List.take p1Card, player2.Tail |> List.take p2Card); status = Playing } with
        | {status = Winner id} -> 
            // printfn "Ending sub game"
            updateHands id (player1, player2)
        | {status = Playing} -> failwith "Game should end with a winner"

    elif p1Card > p2Card then
        updateHands 1 (player1, player2)
    else 
        updateHands 2 (player1, player2)

let rec runGame (previousHands : ((PlayerHand * PlayerHand) list)) ({cards = (player1, player2)} as game : Game) : Game =
    if player1.Length = 0 then
        // printfn "p1 len 0"
        { game with status = Winner 2 }
    elif player2.Length = 0 then
        // printfn "p2 len 0"
        { game with status = Winner 1 }
    elif previousHands |> List.contains game.cards then
        // printfn "Identical game round"
        { game with status = Winner 1 }
    else
        // printfn "Game status"
        // printfn "Player 1's deck: %A" (fst game.cards)
        // printfn "Player 2's deck: %A" (snd game.cards)
        let nextGameState = {
            game with
            cards = playRound runGame (player1, player2)
        }
        runGame ((player1, player2) :: previousHands) nextGameState

let calculateWinnerScore ({cards = (player1, player2)} as game : Game) =
    player1 @ player2
    |> List.rev
    |> List.mapi (fun i x -> (i + 1) * x)
    |> List.sum


let lines =
    readLines "input.txt"
    |> List.ofSeq
    |> parsePlayers
    |> runGame []
    |> calculateWinnerScore 
    |> fun x -> printfn "%A" x; x