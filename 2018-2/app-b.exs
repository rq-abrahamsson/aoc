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
  end

  def almostEqual(array1, array2) do
    result =
      Enum.zip(array1, array2)
      |> Enum.map(fn {a, b} -> a == b end)
      |> Enum.reduce_while([{:one_miss_left, true}, {:result, true}], fn x, acc ->
        [one_miss_left: one_miss_left, result: result] = acc

        cond do
          !x && one_miss_left ->
            {:cont, [{:one_miss_left, false}, {:result, result}]}

          !x && !one_miss_left ->
            {:halt, [{:result, false}]}

          true ->
            {:cont, acc}
        end
      end)

    if result[:one_miss_left] == true && result[:result] do
      [result: false]
    else
      [result: result[:result], array: join(array1, array2)]
    end
  end

  def getAlmostEqual(array, arrays) do
    arrays
    |> Enum.map(fn x -> almostEqual(x, array) end)
    |> Enum.filter(fn x -> x[:result] end)
  end

  def join(array1, array2) do
    Enum.zip(array1, array2)
    |> Enum.filter(fn {a, b} -> a == b end)
    |> Enum.map(fn {a, b} -> a end)
    |> Enum.join("")
  end
end

case File.read("input.txt") do
  {:ok, body} ->
    allStringArrays =
      Solve.toArray(body)
      |> Enum.map(&Solve.getStringData/1)

    # result = Solve.getAlmostEqual(hd(allStringArrays), allStringArrays)
    result =
      allStringArrays
      |> Enum.map(fn x -> Solve.getAlmostEqual(x, allStringArrays) end)
      |> Enum.filter(fn x -> x != [] end)

    IO.inspect(result)

  {:error, reason} ->
    IO.puts(reason)
end
