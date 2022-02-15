part of 'search_bloc.dart';

@freezed
class SearchEvent with _$SearchEvent {
  const factory SearchEvent.started() = _Started;
  const factory SearchEvent.loadMore(String page, String query) = _LoadMore;
  const factory SearchEvent.search(String query) = _Search;
}
