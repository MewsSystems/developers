import 'package:built_collection/built_collection.dart';
import 'package:freezed_annotation/freezed_annotation.dart';

import 'movie.dart';

part 'movies_search_result.freezed.dart';
part 'movies_search_result.g.dart';

@freezed
class MoviesSearchResult with _$MoviesSearchResult {
  const factory MoviesSearchResult({
    int? page,
    List<Movie>? results,
  }) = _MoviesSearchResult;

  factory MoviesSearchResult.fromJson(Map<String, dynamic> json) => _$MoviesSearchResultFromJson(json);
}
