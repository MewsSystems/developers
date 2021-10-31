part of 'list_cubit.dart';

@freezed
class ListState with _$ListState {
  const factory ListState.initial({@Default([]) List<MovieListItem> items}) = Initial;

  const factory ListState.loading({@Default([]) List<MovieListItem> items}) = Loading;

  const factory ListState.loadedData({required List<MovieListItem> items, required int pageNumber}) = LoadedData;

  const factory ListState.error({required List<MovieListItem> items, required String errorMessage}) = Error;

  const factory ListState.allDataLoaded({required List<MovieListItem> items}) = AllDataLoaded;
}
