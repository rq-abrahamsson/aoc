

to_int([], []).
to_int([Y|Tail],[X| Result]) :-
	number_string(X, Y),
	to_int(Tail, Result).

list_increases([_],0).
list_increases([X,Y|TAIL],N) :- 
	X < Y,
	list_increases([Y|TAIL],N1), 
	N is N1 + 1.

list_increases([X,Y|TAIL],N) :- 
	X >= Y,
	list_increases([Y|TAIL],N).

run :-
	read_file_to_string("./input.txt", Input, []),
	split_string(Input, "\n", "", InputList),
	to_int(InputList, InputListAsInt),
	list_increases(InputListAsInt, Result),
	write(Result).

