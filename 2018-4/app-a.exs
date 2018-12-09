defmodule Solve do
  def toArray(body) do
    String.split(body, "\n")
  end

  def parseRow(row) do
    dateString = "#{String.slice(row, 1..16)}:00"
    date = NaiveDateTime.from_iso8601!(dateString)
    [word1 | [word2 | _]] = String.split(String.slice(row, 19..-1), " ")

    event =
      case word1 do
        "Guard" -> {:guard, word2}
        "wakes" -> :wakes
        "falls" -> :falls
      end

    %{
      :date => date,
      :event => event
    }
  end

  def get_sleep_time(guardData) do
    Enum.reduce(guardData, %{}, fn x, acc ->
      if Map.has_key?(acc, :previous_event) && Map.get(acc, :previous_event) == :falls do
        current_date = NaiveDateTime.to_date(Map.get(x, :date))

        if Date.compare(NaiveDateTime.to_date(Map.get(acc, :previous_date)), current_date) == :eq do
          acc
          |> Map.put(
            :time,
            Time.diff(Map.get(x, :date), Map.get(acc, :previous_date)) + Map.get(acc, :time, 0)
          )
          |> Map.put(:previous_date, Map.get(x, :date))
          |> Map.put(:previous_event, Map.get(x, :event))
        else
          acc
          |> Map.put(:previous_date, Map.get(x, :date))
          |> Map.put(:previous_event, Map.get(x, :event))
        end
      else
        acc
        |> Map.put(:previous_date, Map.get(x, :date))
        |> Map.put(:previous_event, Map.get(x, :event))
      end
    end)
  end

  def max_guard(x, acc) do
    if Map.get(x, :time) > Map.get(acc, :time) do
      x
    else
      acc
    end
  end

  def get_minutes_for_time(time) do
    fn guard_time, acc ->
      case Map.get(guard_time, :event) do
        :falls ->
          acc
          |> Map.put(:last_asleep, Map.get(guard_time, :date))

        :wakes ->
          lower = Time.compare(time, Map.get(acc, :last_asleep))
          higher = Time.compare(time, Map.get(guard_time, :date))

          if (lower == :gt || lower == :eq) && higher == :lt do
            Map.put(acc, :number_of_times, Map.get(acc, :number_of_times) + 1)
          else
            acc
          end

        _ ->
          acc
      end
    end
  end

  def solve(body) do
    guards =
      toArray(body)
      |> Enum.map(&parseRow/1)
      |> Enum.sort(&(NaiveDateTime.compare(Map.get(&2, :date), Map.get(&1, :date)) == :gt))
      |> Enum.reduce(%{}, fn x, acc ->
        case Map.get(x, :event) do
          {:guard, id} ->
            if Map.has_key?(acc, id) do
              Map.put(acc, :last_id, id)
            else
              Map.put(Map.put(acc, :last_id, id), id, [])
            end

          :wakes ->
            last_id = Map.get(acc, :last_id)
            Map.put(acc, last_id, Map.get(acc, last_id) ++ [x])

          :falls ->
            last_id = Map.get(acc, :last_id)
            Map.put(acc, last_id, Map.get(acc, last_id) ++ [x])

          _ ->
            acc
        end
      end)
      |> Map.delete(:last_id)

    filtered_guards =
      Map.keys(guards)
      |> Enum.filter(fn x -> Map.get(guards, x) != [] end)
      |> Enum.reduce(%{}, fn x, acc -> Map.put(acc, x, Map.get(guards, x)) end)

    total_time =
      Map.keys(filtered_guards)
      |> Enum.map(fn x ->
        get_sleep_time(Map.get(guards, x))
        |> Map.put(:id, x)
        |> Map.delete(:previous_date)
        |> Map.delete(:previous_event)
      end)
      |> Enum.reduce(&max_guard/2)

    IO.inspect(total_time)
    selected_guard = Map.get(guards, Map.get(total_time, :id))
    IO.inspect(selected_guard)

    max_minute =
      0..59
      |> Enum.map(fn x ->
        case Time.new(0, x, 0, 0) do
          {:ok, time} ->
            Enum.reduce(
              selected_guard,
              %{:number_of_times => 0, :time => x},
              get_minutes_for_time(time)
            )

          {:error, reason} ->
            IO.puts(reason)
        end
      end)
      |> Enum.max_by(fn x -> Map.get(x, :number_of_times) end)

    IO.inspect(max_minute)

    answer =
      String.to_integer(String.slice(Map.get(total_time, :id), 1..-1)) *
        Map.get(max_minute, :time)

    IO.inspect(answer)
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
