
to_int([], []).
to_int([Y|Tail],[X| Result]) :-
	number_string(X, Y),
	to_int(Tail, Result).

split(In, Out) :- string_chars(In, Out).

format_input(Input, RowList) :-
	split_string(Input, ",", "", RowList).


% counter(Elem, List, Result) :-
% 	counter(Elem, List, 0, Result).
    
% counter(_Elem, [], Acc, Acc).
% counter(Elem, [Elem|Tail], Acc, Result) :-
% 	!,
% 	NewAcc is Acc + 1,
% 	counter(Elem, Tail, NewAcc, Result).
% counter(Elem, [_Head|Tail], Acc, Result) :-
% 	counter(Elem, Tail, Acc, Result).

make_zero(_, 8).

create_eight_list(NumberOfElements, Result) :-
	length(NewList, NumberOfElements),
	maplist(make_zero, NewList, Result).

:- use_module(library(clpfd)).

counter(Elem, List, Result) :-
    counter(Elem, List, 0, Result).

counter(_Elem, [], Acc, Acc).
counter(Elem, [Elem|Tail], Acc, Result) :-
    NewAcc #= Acc + 1,
    counter(Elem, Tail, NewAcc, Result).
counter(Elem, [Head|Tail], Acc, Result) :-
    Elem #\= Head,
    counter(Elem, Tail, Acc, Result).


% A

handle_fish_age(X, Y) :-
	X == 0,
	Y is 6.

handle_fish_age(X, Y) :-
	Y is X - 1.

run_generations(Input, Input, Count, Count).

run_generations(Input, Result, Rounds, StartCount) :-
	!,
	maplist(handle_fish_age, Input, UpdatedList),
	NextCount is StartCount + 1,
	counter(6, UpdatedList, NumberOfZeroes),
	create_eight_list(NumberOfZeroes, ZeroList),
	append(UpdatedList, ZeroList, NextRow),
	% write(NextRow),nl,
	run_generations(NextRow, Result, Rounds, NextCount)
	.

solve_a(Input, Result) :-
	to_int(Input, IntList),
	run_generations(IntList, ResultingGeneration, 80, 0),
	length(ResultingGeneration, Result)
	.


run_a :-
	read_file_to_string("./input-test.txt", Input, []),
	format_input(Input, FormattedInput),
	solve_a(FormattedInput, Result),
	write(Result).


% B


solve_b(Result, Result).

run_b :-
	read_file_to_string("./input.txt", Input, []),
	format_input(Input, FormattedInput),
	solve_b(FormattedInput, Result),
	write(Result).
