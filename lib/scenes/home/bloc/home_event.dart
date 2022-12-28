part of 'home_bloc.dart';

@freezed
class HomeEvent with _$HomeEvent {
  const factory HomeEvent.didChangeSearch(String value) =
      HomeEventDidChangeSearch;

  const factory HomeEvent.searchMovies(String searchText) =
      HomeEventSearchMovies;

  const factory HomeEvent.clearList() = HomeEventClearList;

  const factory HomeEvent.retrySearch(String searchText) = HomeEventRetrySearch;
}
