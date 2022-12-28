// ignore_for_file: invalid_annotation_target, depend_on_referenced_packages

import 'package:freezed_annotation/freezed_annotation.dart';

import 'movie.dart';

part 'search_movies_response.freezed.dart';
part 'search_movies_response.g.dart';

@freezed
class SearchMoviesResponse with _$SearchMoviesResponse {
  const SearchMoviesResponse._();

  factory SearchMoviesResponse({
    @JsonKey(name: 'page') required int page,
    @JsonKey(name: 'total_pages') required int totalPages,
    @JsonKey(name: 'total_results') required int totalResults,
    @JsonKey(name: 'results') required List<Movie> movies,
  }) = _SearchMoviesResponse;

  int get nextPage => page < totalPages ? page + 1 : totalPages;

  factory SearchMoviesResponse.fromJson(Map<String, dynamic> json) =>
      _$SearchMoviesResponseFromJson(json);
}
