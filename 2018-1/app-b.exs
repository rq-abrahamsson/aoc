defmodule Solve do
  def toArray(body) do
    String.split(body, "\n")
  end

  def isRepeated() do
    visited_numbers = MapSet.new()

    fn acc, number ->
      next = acc[:number] + number

      cond do
        acc[:found] == true ->
          acc

        MapSet.member?(visited_numbers, next) ->
          [{:found, true}, {:number, next}]

        true ->
          MapSet.put(visited_numbers, next)
          [{:found, false}, {:number, next}]
      end
    end
  end
end

case File.read("input-b.txt") do
  {:ok, body} ->
    list = Enum.map(Solve.toArray(body), fn x -> String.to_integer(x) end)

    result =
      Stream.transform(
        Stream.cycle(list),
        [{:visited_numbers, MapSet.new()}, {:number, 0}],
        fn x, acc ->
          [visited_numbers: visited_numbers, number: number] = acc
          newNumber = number + x

          if MapSet.member?(visited_numbers, newNumber) do
            IO.inspect(newNumber)
            {:halt, [newNumber]}
          else
            {[x],
             [{:visited_numbers, MapSet.put(visited_numbers, newNumber)}, {:number, newNumber}]}
          end
        end
      )

    IO.inspect(Enum.to_list(result))

  {:error, reason} ->
    IO.puts(reason)
end
