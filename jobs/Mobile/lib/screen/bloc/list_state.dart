part of 'list_cubit.dart';

@freezed
class ListState with _$ListState {
  const factory ListState.initial() = Initial;

  const factory ListState.loading() = Loading;

  const factory ListState.loadedData(List<MovieListItem> items) = LoadedData;
  const factory ListState.error(List<MovieListItem> items, String errorMessage) = Error;
}
