
to_int([], []).
to_int([Y|Tail],[X| Result]) :-
	number_string(X, Y),
	to_int(Tail, Result).

split(In, Out) :- split_string(In, " ", "", Out).


solve_a([], 0, 0).
solve_a([["forward", Value]|Tail], SumForward, ODown) :-
	number_string(NValue, Value),
	solve_a(Tail, OForward, ODown),
	SumForward is NValue + OForward.
solve_a([["down", Value]|Tail], OForward, SumDown) :-
	number_string(NValue, Value),
	solve_a(Tail, OForward, ODown),
	SumDown is NValue + ODown.
solve_a([["up", Value]|Tail], OForward, SumDown) :-
	number_string(NValue, Value),
	solve_a(Tail, OForward, ODown),
	SumDown is ODown - NValue.

run_a :-
	read_file_to_string("./input.txt", Input, []),
	split_string(Input, "\n", "", RowList),
	maplist(split, RowList, RowList2),
	solve_a(RowList2, Forward, Down),
	Result is Forward * Down,
	write(Result).


solve_b(Input, Result) :-
	Result is Input.

run_b :-
	read_file_to_string("./input.txt", Input, []),
	split_string(Input, "\n", "", InputList),
	to_int(InputList, InputListAsInt),
	solve_b(InputListAsInt, Result),
	write(Result).
