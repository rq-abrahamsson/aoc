defmodule Solve do
  def toArray(body) do
    String.split(body, "\n")
  end

  def toSquare(string) do
    [id | [_ | [coords | [size]]]] = String.split(string, " ")
    [x | [y | _]] = String.split(coords, ~r/:|,/)
    [height | [width]] = String.split(size, "x")

    %{
      :id => id,
      :x => String.to_integer(x),
      :y => String.to_integer(y),
      :height => String.to_integer(height),
      :width => String.to_integer(width)
    }
  end

  def squareToCoordIdList(square) do
    List.flatten(
      Enum.map(square[:y]..(square[:y] + square[:width] - 1), fn x ->
        Enum.map(square[:x]..(square[:x] + square[:height] - 1), fn y ->
          %{coord: "#{Integer.to_string(x)}-#{Integer.to_string(y)}", id: square[:id]}
        end)
      end)
    )
  end

  def getDoubles(x, acc) do
    if Map.has_key?(Keyword.get(acc, :single), x.coord) do
      acc = Keyword.put(acc, :double, MapSet.put(Keyword.get(acc, :double), x.id))

      acc =
        Keyword.put(
          acc,
          :double,
          MapSet.put(Keyword.get(acc, :double), Map.get(Keyword.get(acc, :single), x.coord))
        )

      acc
    else
      acc = Keyword.put(acc, :single, Map.put(Keyword.get(acc, :single), x.coord, x.id))
      acc
    end
  end

  def solve() do
    case File.read("input.txt") do
      {:ok, body} ->
        squareList =
          Solve.toArray(body)
          |> Enum.filter(fn x -> x != "" end)
          |> Enum.map(&Solve.toSquare/1)

        coordIdList =
          List.flatten(
            squareList
            |> Enum.map(&Solve.squareToCoordIdList/1)
          )

        overlappingIds =
          coordIdList
          |> Enum.reduce(
            [{:single, %{}}, {:double, MapSet.new()}],
            &Solve.getDoubles/2
          )
          |> Keyword.get(:double)

        idList = squareList |> Enum.map(fn x -> x.id end)
        IO.inspect(Enum.find(idList, fn x -> !MapSet.member?(overlappingIds, x) end))

      {:error, reason} ->
        IO.puts(reason)
    end
  end
end

Solve.solve()
