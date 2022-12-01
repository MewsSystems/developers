part of 'search_bloc.dart';

abstract class SearchEvent extends Equatable {
  const SearchEvent();

  @override
  List<Object> get props => [];
}

class FirstSearchEvent extends SearchEvent {
  const FirstSearchEvent(this.query);

  final String query;

  @override
  List<Object> get props => [query];
}

class NextSearchEvent extends SearchEvent {}
