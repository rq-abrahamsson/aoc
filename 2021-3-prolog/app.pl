
to_int([], []).
to_int([Y|Tail],[X| Result]) :-
	number_string(X, Y),
	to_int(Tail, Result).

split(In, Out) :- string_chars(In, Out).

range_binary_value(X,X,Y,[Y]) :- !.
range_binary_value(Low, High, IndexPower, [IndexPower|Xs]) :-
    Low =< High,
    NewLow is Low+1,
    NextIndexPower is IndexPower * 2,
    range_binary_value(NewLow, High, NextIndexPower, Xs).


format_input(Input, FormattedInput) :-
	split_string(Input, "\n", "", RowList),
	maplist(split, RowList, FormattedInput).

% A

get_rest([_|Rest], Rest).

get_first([First|_], First).

get_first_and_rest_lists(T, AllFirsts, AllRests) :-
	maplist(get_first, T, AllFirsts),
	maplist(get_rest, T, AllRests).


count_numbers([], 0, 0).
count_numbers([Head | Tail], NewOneCount, ZeroCount) :-
	Head = '1',
	count_numbers(Tail, OneCount, ZeroCount),
	NewOneCount is 1 + OneCount.
count_numbers([Head | Tail], OneCount, NewZeroCount) :-
	Head = '0',
	count_numbers(Tail, OneCount, ZeroCount),
	NewZeroCount is 1 + ZeroCount.

new_stuff([[]|_], []).
new_stuff(Input, Result) :-
	get_first_and_rest_lists(Input, FirstElem, RestElems),
	new_stuff(RestElems, RestRows),
	append([FirstElem], RestRows, Result).

gamma(OneCount, ZeroCount, ResultingNumber) :-
	OneCount > ZeroCount,
	ResultingNumber is 1.
gamma(OneCount, ZeroCount, ResultingNumber) :-
	OneCount < ZeroCount,
	ResultingNumber is 0.

epsilon(OneCount, ZeroCount, ResultingNumber) :-
	OneCount < ZeroCount,
	ResultingNumber is 1.
epsilon(OneCount, ZeroCount, ResultingNumber) :-
	OneCount > ZeroCount,
	ResultingNumber is 0.


get_gamma(Ones, Zeroes, Gamma) :-
	maplist(gamma, Ones, Zeroes, Gamma).

get_epsilon(Ones, Zeroes, Epsilon) :-
	maplist(epsilon, Ones, Zeroes, Epsilon).

get_count_list([], [], []).
get_count_list([Head | Tail], OutOneList, OutZeroList) :-
	count_numbers(Head, OneCount, ZeroCount),
	get_count_list(Tail, OutOnes, OutZeroes),
	append([OneCount], OutOnes, OutOneList),
	append([ZeroCount], OutZeroes, OutZeroList).

product(X, Y, Z) :-
	Z is X * Y.

to_decimal(ListNumber, Result) :-
	reverse(ListNumber, ReversedNumber),
	length(ReversedNumber, Length),
	L is Length-1,
	range_binary_value(0, L, 1, Range),
	maplist(product, ReversedNumber, Range, Products),
	foldl(plus, Products, 0, Result).

solve_a(Input, Result) :-
	new_stuff(Input, OutLists),
	get_count_list(OutLists, Ones, Zeroes),
	get_gamma(Ones, Zeroes, Gamma),
	get_epsilon(Ones, Zeroes, Epsilon),
	to_decimal(Gamma, GammaDecimal),
	to_decimal(Epsilon, EpsilonDecimal),
	Result is GammaDecimal * EpsilonDecimal.


run_a :-
	read_file_to_string("./input.txt", Input, []),
	format_input(Input, FormattedInput),
	% write(FormattedInput),nl.
	solve_a(FormattedInput, Result),
	write(Result).


% B


solve_b(Input, Result) :-
	Result is Input.

run_b :-
	read_file_to_string("./input.txt", Input, []),
	format_input(Input, FormattedInput),
	solve_b(FormattedInput, Result),
	write(Result).
