#r "nuget: FSharpPlus"
open FSharpPlus

let readLines filePath = System.IO.File.ReadLines(filePath);

type Food = {
    ingredients: string list
    allergens: string list
}

let parseFood line =
    let list =
        line
        |> String.split [" "; ")"]
        |> Seq.toList

    let ingredients =
        list
        |> List.takeWhile (fun x -> x <> "(contains")
    let allergens =
        list
        |> List.skipWhile (fun x -> x <> "(contains")
        |> List.skip 1
        |> List.takeWhile (fun x -> x <> "")
        |> List.map (fun x -> x |> String.trim ",")

    {
        ingredients = ingredients
        allergens = allergens
    }

let getAllergenList (l : Food list) =
    l
    |> List.map (fun {allergens = allergens} -> allergens)
    |> List.concat
    |> List.distinct

let getIngredientList (l : Food list) =
    l
    |> List.map (fun {ingredients = ingredients} -> ingredients)
    |> List.concat
    |> List.distinct

let getPossibleIngredientNameForAllergen foods allergen =
    let allIngredients =
        foods
        |> getIngredientList
    foods
    |> List.filter (fun food -> food.allergens |> List.contains allergen)
    |> List.map (fun food -> food.ingredients)
    |> List.map Set.ofList
    |> List.fold Set.intersect (allIngredients |> Set.ofList)
    |> Seq.toList

let rec getIngredientNames (l : (string * string list) list) =
    if l |> List.forall (fun (_, l) -> l |> List.length = 1) then
        l
    else
        let determinedIngredients = l |> List.filter (fun (_, l) -> l.Length = 1)
        let foundIngredients =
            determinedIngredients
            |> List.map (fun (x, l) -> l.[0])
        let restIngredients =
            l
            |> List.filter (fun (_, l) -> l.Length > 1)
            |> List.map (fun (x, l) -> (x, l |> List.filter (fun p -> not (foundIngredients |> List.contains p))))
        getIngredientNames (List.concat [determinedIngredients; restIngredients])

let foodList = 
    readLines "input.txt"
    |> Seq.map parseFood
    |> Seq.toList

let ingredientMapping =
    foodList
    |> getAllergenList
    |> List.map (fun x -> (x, getPossibleIngredientNameForAllergen foodList x))
    |> getIngredientNames
    |> List.map (fun (x, h::_) -> (x, h))

foodList
    |> List.map (fun x -> x.ingredients)
    |> List.concat
    |> List.filter (fun x -> not (ingredientMapping |> List.map (fun (_, y) -> y) |> List.contains x))
    |> List.length
    |> fun x -> printfn "%A" x; x