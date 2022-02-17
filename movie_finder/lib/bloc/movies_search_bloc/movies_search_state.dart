part of 'movies_search_bloc.dart';

@freezed
class MoviesSearchState with _$MoviesSearchState {
  const factory MoviesSearchState.initial() = MoviesSearchStateInitial;
  const factory MoviesSearchState.loading() = MoviesSearchStateLoading;
  const factory MoviesSearchState.success(List<Movie> movies) = MoviesSearchStateSuccess;
  const factory MoviesSearchState.noMorePages(List<Movie> movies) = MoviesSearchStateNoMorePages;
  const factory MoviesSearchState.error(Object error) = MoviesSearchStateError;
}
