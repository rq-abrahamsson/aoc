defmodule Solve do
  def toArray(body) do
    String.split(body, "\n")
  end

  def countLetters(x, acc) do
    if Map.has_key?(acc, x) do
      value = Map.get(acc, x)
      Map.put(acc, x, value + 1)
    else
      Map.put(acc, x, 1)
    end
  end

  def twosAndThrees(x, acc) do
    if x == 2 || x == 3 do
      Map.put(acc, x, 1)
    else
      acc
    end
  end

  def getStringData(string) do
    string
    |> String.split("")
    |> Enum.filter(fn x -> x != "" end)
    |> Enum.reduce(%{}, &Solve.countLetters/2)
    |> Map.values()
    |> Enum.uniq()
    |> Enum.reduce(%{2 => 0, 3 => 0}, &Solve.twosAndThrees/2)
  end
end

case File.read("input.txt") do
  {:ok, body} ->
    result =
      Solve.toArray(body)
      |> Enum.map(&Solve.getStringData/1)
      |> Enum.reduce(%{2 => 0, 3 => 0}, fn x, acc ->
        %{2 => acc[2] + x[2], 3 => acc[3] + x[3]}
      end)

    IO.inspect(result)
    answer = result[2] * result[3]
    IO.inspect(answer)

  {:error, reason} ->
    IO.puts(reason)
end
