import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movie_search/model/movie_list_item/movie_list_item.dart';

part 'movies_list_response.freezed.dart';
part 'movies_list_response.g.dart';

@freezed
class MoviesListResponse with _$MoviesListResponse {
  const MoviesListResponse._();

  const factory MoviesListResponse(
      {required int page,
      @JsonKey(name: 'total_pages') required int totalPages,
      @JsonKey(name: 'total_results') required int totalResults,
      @JsonKey(name: 'results') required List<MovieListItem> items}) = _MovieListResponse;

  factory MoviesListResponse.fromJson(Map<String, dynamic> json) => _$MoviesListResponseFromJson(json);
}
