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

  def squareToCoordList(square) do
    List.flatten(
      Enum.map(square[:y]..(square[:y] + square[:width] - 1), fn x ->
        Enum.map(square[:x]..(square[:x] + square[:height] - 1), fn y ->
          "#{Integer.to_string(x)}-#{Integer.to_string(y)}"
        end)
      end)
    )
  end

  def getDoubles(x, acc) do
    if MapSet.member?(Keyword.get(acc, :single), x) do
      acc = Keyword.put(acc, :double, MapSet.put(Keyword.get(acc, :double), x))
      acc
    else
      acc = Keyword.put(acc, :single, MapSet.put(Keyword.get(acc, :single), x))
      acc
    end
  end

  def solve() do
    case File.read("input.txt") do
      {:ok, body} ->
        coordList =
          Solve.toArray(body)
          |> Enum.filter(fn x -> x != "" end)
          |> Enum.map(&Solve.toSquare/1)
          |> Enum.map(&Solve.squareToCoordList/1)

        result =
          List.flatten(coordList)
          |> Enum.reduce(
            [{:single, MapSet.new()}, {:double, MapSet.new()}],
            &Solve.getDoubles/2
          )

        IO.inspect(MapSet.size(Keyword.get(result, :double)))

      {:error, reason} ->
        IO.puts(reason)
    end
  end
end

Solve.solve()
