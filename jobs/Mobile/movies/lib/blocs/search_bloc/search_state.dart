part of 'search_bloc.dart';

abstract class SearchState extends Equatable {
  const SearchState();

  @override
  List<Object> get props => [];
}

class InitialSearchState extends SearchState {}

class LoadingSearchState extends SearchState {}

class SuccessSearchState extends SearchState {
  const SuccessSearchState(
    this.page,
    this.query,
    this.movies,
    this.total,
    this.hasMorePages,
  );

  final int page;
  final String query;
  final List<Movie> movies;
  final int total;
  final bool hasMorePages;

  @override
  List<Object> get props => [page, query, movies, total, hasMorePages];

  @override
  String toString() =>
      'page: $page, query: $query, length: ${movies.length} hasMorePages: $hasMorePages';
}

class ErrorSearchState extends SearchState {
  const ErrorSearchState({required this.message});

  final String message;

  @override
  List<Object> get props => [message];
}
