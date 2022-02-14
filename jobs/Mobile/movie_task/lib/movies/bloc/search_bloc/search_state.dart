part of 'search_bloc.dart';

enum SearchStatus { initial, success, failure }

class SearchState extends Equatable {
  const SearchState({
    this.status = SearchStatus.initial,
    this.movies = const <Movie>[],
    this.hasReachedMax = false,
  });

  final SearchStatus status;
  final List<Movie> movies;
  final bool hasReachedMax;

  SearchState copyWith({
    SearchStatus? status,
    List<Movie>? movies,
    bool? hasReachedMax,
  }) =>
      SearchState(
        status: status ?? this.status,
        movies: movies ?? this.movies,
        hasReachedMax: hasReachedMax ?? this.hasReachedMax,
      );

  @override
  String toString() =>
      '''SearchState { status: $status, hasReachedMax: $hasReachedMax, movies: ${movies.length} }''';

  @override
  List<Object> get props => [
        status,
        movies,
        hasReachedMax,
      ];
}
