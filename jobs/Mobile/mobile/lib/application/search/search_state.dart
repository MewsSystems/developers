part of 'search_bloc.dart';

@freezed
class SearchState with _$SearchState {
  const factory SearchState({
    required String err,
    required Status status,
    required num lastPage,
    required List<Movie> result,
  }) = _SearchState;

  factory SearchState.initial() => SearchState(
      lastPage: 0, status: Status.waiting, err: '', result: [Movie.empty()]);
}
