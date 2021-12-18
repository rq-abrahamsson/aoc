:- use_module(library(clpfd)).
:- use_module(library(lists)).

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

is_intersect_length(Str1, Str2, Len) :-
	string_codes(Str1, Str1L),
	string_codes(Str2, Str2L),
	intersection(Str1L, Str2L, Intersection),
	length(Intersection, Length),
	Length #= Len.

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

is_one(_Acc, Str, Str) :-
	string_length(Str, Len),
	Len #= 2.

is_one(Acc, _Str, Acc).

get_one(StringList, Result) :-
	foldl(is_one, StringList, "", Result).


is_seven(_Acc, Str, Str) :-
	string_length(Str, Len),
	Len #= 3.

is_seven(Acc, _Str, Acc).

get_seven(StringList, Result) :-
	foldl(is_seven, StringList, "", Result).

is_four(_Acc, Str, Str) :-
	string_length(Str, Len),
	Len #= 4.

is_four(Acc, _Str, Acc).

get_four(StringList, Result) :-
	foldl(is_four, StringList, "", Result).

is_eight(_Acc, Str, Str) :-
	string_length(Str, Len),
	Len #= 7.

is_eight(Acc, _Str, Acc).

get_eight(StringList, Result) :-
	foldl(is_eight, StringList, "", Result).

is_two(One, Four, Seven, _Acc, Str, Str) :-
	is_intersect_length(Str, One, 1),
	is_intersect_length(Str, Four, 2),
	is_intersect_length(Str, Seven, 2).

is_two(_One, _Four, _Seven, Acc, _Str, Acc).

get_two(StringList, One, Four, Seven, Result) :-
	foldl(is_two(One, Four, Seven), StringList, "", Result).

is_three(One, Two, Four, Seven, _Acc, Str, Str) :-
	is_intersect_length(Str, One, 2),
	is_intersect_length(Str, Two, 4),
	is_intersect_length(Str, Four, 3),
	is_intersect_length(Str, Seven, 3).
	

is_three(_One, _Two, _Four, _Seven, Acc, _Str, Acc).

get_three(StringList, One, Two, Four, Seven, Result) :-
	foldl(is_three(One, Two, Four, Seven), StringList, "", Result).

is_five(One, Two, Three, Four, Seven, _Acc, Str, Str) :-
	is_intersect_length(Str, One, 1),
	is_intersect_length(Str, Two, 3),
	is_intersect_length(Str, Three, 4),
	is_intersect_length(Str, Four, 3),
	is_intersect_length(Str, Seven, 2).
	

is_five(_One, _Two, _Three, _Four, _Seven, Acc, _Str, Acc).

get_five(StringList, One, Two, Three, Four, Seven, Result) :-
	foldl(is_five(One, Two, Three, Four, Seven), StringList, "", Result).

is_six(One, Two, Three, Four, Five, Seven, _Acc, Str, Str) :-
	is_intersect_length(Str, One, 1),
	is_intersect_length(Str, Two, 4),
	is_intersect_length(Str, Three, 4),
	is_intersect_length(Str, Four, 3),
	is_intersect_length(Str, Five, 5),
	is_intersect_length(Str, Seven, 2).
	

is_six(_One, _Two, _Three, _Four, _Five, _Seven, Acc, _Str, Acc).

get_six(StringList, One, Two, Three, Four, Five, Seven, Result) :-
	foldl(is_six(One, Two, Three, Four, Five, Seven), StringList, "", Result).


is_nine(One, Two, Three, Four, Five, Six, Seven, _Acc, Str, Str) :-
	is_intersect_length(Str, One, 2),
	is_intersect_length(Str, Two, 4),
	is_intersect_length(Str, Three, 5),
	is_intersect_length(Str, Four, 4),
	is_intersect_length(Str, Five, 5),
	is_intersect_length(Str, Six, 5),
	is_intersect_length(Str, Seven, 3).
	

is_nine(_One, _Two, _Three, _Four, _Five, _Six, _Seven, Acc, _Str, Acc).

get_nine(StringList, One, Two, Three, Four, Five, Six, Seven, Result) :-
	foldl(is_nine(One, Two, Three, Four, Five, Six, Seven), StringList, "", Result).

is_zero(One, Two, Three, Four, Five, Six, Seven, Nine, _Acc, Str, Str) :-
	is_intersect_length(Str, One, 2),
	is_intersect_length(Str, Two, 4),
	is_intersect_length(Str, Three, 4),
	is_intersect_length(Str, Four, 3),
	is_intersect_length(Str, Five, 4),
	is_intersect_length(Str, Six, 5),
	is_intersect_length(Str, Seven, 3),
	is_intersect_length(Str, Nine, 5).
	

is_zero(_One, _Two, _Three, _Four, _Five, _Six, _Seven, _Nine, Acc, _Str, Acc).

get_zero(StringList, One, Two, Three, Four, Five, Six, Seven, Nine, Result) :-
	foldl(is_zero(One, Two, Three, Four, Five, Six, Seven, Nine), StringList, "", Result).

numbers(Input, [Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine]) :-
	get_one(Input, One),
	get_four(Input, Four),
	get_seven(Input, Seven),
	get_eight(Input, Eight),
	get_two(Input, One, Four, Seven, Two),
	get_three(Input, One, Two, Four, Seven, Three),
	get_five(Input, One, Two, Three, Four, Seven, Five),
	get_six(Input, One, Two, Three, Four, Five, Seven, Six),
	get_nine(Input, One, Two, Three, Four, Five, Six, Seven, Nine),
	get_zero(Input, One, Two, Three, Four, Five, Six, Seven, Nine, Zero).

solve_b(Result, Result).

run_b :-
	read_file_to_string("./input.txt", Input, []),
	format_input(Input, FormattedInput),
	solve_b(FormattedInput, Result),
	write(Result).
