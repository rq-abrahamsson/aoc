defmodule Solve do
  def toArray(body) do
    String.split(String.trim(body), "")
  end

  def removeReactions(x, acc) do
    if acc != [] do
      first = hd(acc)

      if reacts?(first, x) do
        tl(acc)
      else
        [x] ++ acc
      end
    else
      [x]
    end
  end

  def getUniqLetters(string) do
    string
    |> String.downcase()
    |> toArray()
    |> Enum.filter(fn x -> x != "" end)
    |> Enum.uniq()
  end

  def getLetterValue(letter, string) do
    string
    |> toArray()
    |> Enum.filter(fn x -> x != "" end)
    |> Enum.filter(fn x -> x != String.upcase(letter) && x != letter end)
    |> Enum.reduce([], &removeReactions/2)
    |> Enum.join("")
    |> String.length()
  end

  def solve(input) do
    result =
      input
      |> getUniqLetters()
      |> Enum.map(fn letter ->
        getLetterValue(letter, input)
      end)
      |> Enum.min()

    IO.inspect(result)
  end

  def reacts?(letter1, letter2) do
    !String.equivalent?(letter1, letter2) &&
      (String.equivalent?(String.downcase(letter1), letter2) ||
         String.equivalent?(letter1, String.downcase(letter2)))
  end

  def run() do
    case File.read("input.txt") do
      {:ok, body} ->
        solve(body)

      {:error, reason} ->
        IO.puts(reason)
    end
  end
end

Solve.run()
