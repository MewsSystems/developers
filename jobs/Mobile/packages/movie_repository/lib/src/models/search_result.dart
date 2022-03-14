import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';
import 'package:movie_repository/movie_repository.dart';

part 'search_result.g.dart';

@JsonSerializable()
class SearchResult extends Equatable {
  final List<MoviePreview> previews;
  final int totalPages;
  final int totalResults;

  const SearchResult({
    required this.previews,
    required this.totalPages,
    required this.totalResults,
  });

  factory SearchResult.fromJson(json) => _$SearchResultFromJson(json);

  Map<String, dynamic> toJson() => _$SearchResultToJson(this);

  @override
  List<Object> get props => [previews, totalPages, totalResults];
}
