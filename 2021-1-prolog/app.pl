

to_int([], []).
to_int([Y|Tail],[X| Result]) :-
	number_string(X, Y),
	to_int(Tail, Result).

list_increases([_],0).
list_increases([X,Y|Tail],N) :-
	X < Y,
	list_increases([Y|Tail],N1),
	N is N1 + 1.

list_increases([X,Y|Tail],N) :-
	X >= Y,
	list_increases([Y|Tail],N).

run_a :-
	read_file_to_string("./input.txt", Input, []),
	split_string(Input, "\n", "", InputList),
	to_int(InputList, InputListAsInt),
	list_increases(InputListAsInt, Result),
	write(Result).

windowed_sum([_],[]).
windowed_sum([_,_],[]).
windowed_sum([A,B,C|Tail], [X|Output]) :-
	X is A + B + C,
	windowed_sum([B,C|Tail], Output).


solve_b(Input, Result) :-
	windowed_sum(Input, WindowedSumList),
	list_increases(WindowedSumList, Result).

run_b :-
	read_file_to_string("./input.txt", Input, []),
	split_string(Input, "\n", "", InputList),
	to_int(InputList, InputListAsInt),
	solve_b(InputListAsInt, Result),
	write(Result).


% to_int(["1", "2"], Res).
% to_int(Res, [1, 2]).