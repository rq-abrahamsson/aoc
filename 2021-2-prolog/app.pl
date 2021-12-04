
to_int([], []).
to_int([Y|Tail],[X| Result]) :-
	number_string(X, Y),
	to_int(Tail, Result).

split(In, Out) :- split_string(In, " ", "", Out).

format_input(Input, FormattedInput) :-
	split_string(Input, "\n", "", RowList),
	maplist(split, RowList, FormattedInput).

% A

handle_command_a([], 0, 0).
handle_command_a([["forward", Value]|Tail], SumForward, ODown) :-
	number_string(NValue, Value),
	handle_command_a(Tail, OForward, ODown),
	SumForward is NValue + OForward.
handle_command_a([["down", Value]|Tail], OForward, SumDown) :-
	number_string(NValue, Value),
	handle_command_a(Tail, OForward, ODown),
	SumDown is NValue + ODown.
handle_command_a([["up", Value]|Tail], OForward, SumDown) :-
	number_string(NValue, Value),
	handle_command_a(Tail, OForward, ODown),
	SumDown is ODown - NValue.


solve_a(Input, Result) :-
	handle_command_a(Input, Forward, Down),
	Result is Forward * Down.

run_a :-
	read_file_to_string("./input.txt", Input, []),
	format_input(Input, FormattedInput),
	solve_a(FormattedInput, Result),
	write(Result).


% B

handle_command_b([], _, 0, 0).
handle_command_b([["forward", Value]|Tail], Aim, SumForward, ResultDown) :-
	number_string(NValue, Value),
	handle_command_b(Tail, Aim, OForward, ODown),
	ResultDown is (Aim * NValue) + ODown,
	SumForward is NValue + OForward.
handle_command_b([["down", Value]|Tail], Aim, OForward, ODown) :-
	number_string(NValue, Value),
	InAimDown is NValue + Aim,
	handle_command_b(Tail, InAimDown, OForward, ODown).
handle_command_b([["up", Value]|Tail], Aim, OForward, ODown) :-
	number_string(NValue, Value),
	InAimDown is Aim - NValue,
	handle_command_b(Tail, InAimDown, OForward, ODown).


solve_b(Input, Result) :-
	handle_command_b(Input, 0, Forward, Down),
	Result is Forward * Down.

run_b :-
	read_file_to_string("./input.txt", Input, []),
	format_input(Input, FormattedInput),
	solve_b(FormattedInput, Result),
	write(Result).
