defmodule Solve do
  def toArray(body) do
    String.split(String.trim(body), "")
  end

  def removeReactions(x, acc) do
    if acc != [] do
      first = hd(acc)

      if Solve.reacts?(first, x) do
        tl(acc)
      else
        [x] ++ acc
      end
    else
      [x]
    end
  end

  def solve(body) do
    tmp =
      toArray(body)
      |> Enum.filter(fn x -> x != "" end)
      |> Enum.reduce([], &Solve.removeReactions/2)

    IO.inspect(length(tmp))
  end

  def reacts?(letter1, letter2) do
    !String.equivalent?(letter1, letter2) &&
      (String.equivalent?(String.downcase(letter1), letter2) ||
         String.equivalent?(letter1, String.downcase(letter2)))
  end

  def run() do
    case File.read("input.txt") do
      {:ok, body} ->
        Solve.solve(body)

      {:error, reason} ->
        IO.puts(reason)
    end
  end
end

Solve.run()
