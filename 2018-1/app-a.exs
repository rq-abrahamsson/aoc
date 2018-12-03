defmodule Solve do
  def toArray(body) do
    String.split(body, "\n")
  end
end

case File.read("input-a.txt") do
  {:ok, body} ->
    IO.puts(Enum.reduce(Enum.map(Solve.toArray(body), fn x -> String.to_integer(x) end), 0, &+/2))

  {:error, reason} ->
    IO.puts(reason)
end
