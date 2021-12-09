
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


solve_b(Result, Result).

run_b :-
	read_file_to_string("./input.txt", Input, []),
	format_input(Input, FormattedInput),
	solve_b(FormattedInput, Result),
	write(Result).
