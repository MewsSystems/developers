part of 'search_bloc.dart';

class SearchState extends Equatable {
  const SearchState({
    this.query = '',
    this.status = SearchStatus.initial,
    this.previews = const <MoviePreview>[],
    this.hasReachedMax = false,
    this.page = 1,
    this.maxPage = 1,
  });

  final String query;
  final SearchStatus status;
  final List<MoviePreview> previews;
  final int page;
  final bool hasReachedMax;
  final int maxPage;

  @override
  List<Object> get props => [query, status, previews, hasReachedMax, page];

  SearchState copyWith({
    String? query,
    SearchStatus? status,
    List<MoviePreview>? previews,
    int? page,
    bool? hasReachedMax,
    int? maxPage,
  }) =>
      SearchState(
        query: query ?? this.query,
        status: status ?? this.status,
        previews: previews ?? this.previews,
        page: page ?? this.page,
        hasReachedMax: hasReachedMax ?? this.hasReachedMax,
        maxPage: maxPage ?? this.maxPage,
      );
}
