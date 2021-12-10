:- use_module(library(clpfd)).

to_int([], []).
to_int([Y|Tail],[X| Result]) :-
	number_string(X, Y),
	to_int(Tail, Result).

split(In, Out) :- string_chars(In, Out).

format_input_rows(Input, RowList) :-
	split_string(Input, "\n", "", RowList).

format_input_first_pass(Input, RowList) :-
	split_string(Input, "|", " ", RowList).

format_input_second_pass(Input, RowList) :-
	split_string(Input, " ", "", RowList).

get_second_list_element([_Head,Next], Next).
get_second_list_element([_Head,Next], Next).

format_input(Input, Result) :-
	format_input_rows(Input, Rows),
	maplist(format_input_first_pass, Rows, SplitRows),
	maplist(get_second_list_element, SplitRows , OutputValues),
	maplist(format_input_second_pass, OutputValues, Result).

different(X, X) :- !, fail.
different(_X, _Y).

different(A, A, _, _, _, _, _) :- !, fail.
different(A, _, A, _, _, _, _) :- !, fail.
different(A, _, _, A, _, _, _) :- !, fail.
different(A, _, _, _, A, _, _) :- !, fail.
different(A, _, _, _, _, A, _) :- !, fail.
different(A, _, _, _, _, _, A) :- !, fail.
different(_, B, B, _, _, _, _) :- !, fail.
different(_, B, _, B, _, _, _) :- !, fail.
different(_, B, _, _, B, _, _) :- !, fail.
different(_, B, _, _, _, B, _) :- !, fail.
different(_, B, _, _, _, _, B) :- !, fail.
different(_, _, C, C, _, _, _) :- !, fail.
different(_, _, C, _, C, _, _) :- !, fail.
different(_, _, C, _, _, C, _) :- !, fail.
different(_, _, C, _, _, _, C) :- !, fail.
different(_, _, _, D, D, _, _) :- !, fail.
different(_, _, _, D, _, D, _) :- !, fail.
different(_, _, _, D, _, _, D) :- !, fail.
different(_, _, _, _, E, E, _) :- !, fail.
different(_, _, _, _, E, _, E) :- !, fail.
different(_, _, _, _, _, F, F) :- !, fail.
different(_A, _B, _C, _D, _E, _F, _G).


% A



get_string_length(Input, Output) :-
	maplist(atom_length, Input, Output).


unique_mapper(2, 1).
unique_mapper(4, 1).
unique_mapper(3, 1).
unique_mapper(7, 1).
unique_mapper(_, 0).


count_unique_digits(Input, Output) :-
	maplist(unique_mapper, Input, R),
	foldl(plus, R, 0, Output).
	

solve_a(Input, Result) :-
	maplist(get_string_length, Input, R),
	maplist(count_unique_digits, R, R1),
	foldl(plus, R1, 0, Result).


run_a :-
	read_file_to_string("./input.txt", Input, []),
	format_input(Input, FormattedInput),
	solve_a(FormattedInput, Result),
	write(Result).


% B



% acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |
% cdfeb fcadb cdfeb cdbaf

% one_is(a, b).
% four_is(e, a, f, b).
% seven_is(d, a, b).
% eight_is(a, c, e, d, g, f, b).

one(a).
one(b).

four(e).
four(a).
four(f).
four(b).

seven(d).
seven(a).
seven(b).

eight(a).
eight(c).
eight(e).
eight(d).
eight(g).
eight(f).
eight(b).

a_position(X) :-
	eight(X),
	seven(X),
	not(one(X)),
	not(four(X)).

b_position(X) :-
	eight(X),
	four(X),
	not(one(X)),
	not(seven(X)).

c_position(X) :-
	eight(X),
	one(X),
	four(X),
	seven(X).

d_position(X) :-
	eight(X),
	four(X),
	not(seven(X)),
	not(one(X)).

e_position(X) :-
	eight(X),
	not(four(X)),
	not(seven(X)),
	not(one(X)).

f_position(X) :-
	eight(X),
	four(X),
	seven(X),
	one(X).

g_position(X) :-
	eight(X),
	not(four(X)),
	not(seven(X)),
	not(one(X)).


numbers(A, B, C, D, E, F, G) :-
	a_position(A),
	b_position(B),
	c_position(C),
	d_position(D),
	e_position(E),
	f_position(F),
	g_position(G),
	different(A, B, C, D, E, F, G).






solve_b(Result, Result).

run_b :-
	read_file_to_string("./input.txt", Input, []),
	format_input(Input, FormattedInput),
	solve_b(FormattedInput, Result),
	write(Result).
