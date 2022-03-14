import 'package:json_annotation/json_annotation.dart';
import 'package:tmdb_api/src/models/models.dart';

part 'search_result.g.dart';

@JsonSerializable()
class SearchResult {
  final int page;
  final List<MoviePreview> results;
  final int totalResults;
  final int totalPages;
  SearchResult({
    required this.page,
    required this.results,
    required this.totalResults,
    required this.totalPages,
  });

  factory SearchResult.fromJson(Map<String, dynamic> json) =>
      _$SearchResultFromJson(json);

  Map<String, dynamic> toJson() => _$SearchResultToJson(this);
}
