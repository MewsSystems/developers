part of 'search_bloc.dart';

abstract class SearchEvent extends Equatable {
  const SearchEvent();

  @override
  List<Object> get props => [];
}

class FirstSearchEvent extends SearchEvent {
  const FirstSearchEvent(this.query, this.page);

  final String query;
  final int page;

  @override
  List<Object> get props => [query, page];
}

class NextSearchEvent extends SearchEvent {}
