import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';
import 'package:movies/models/movie_model.dart';

part 'movie_search_response_model.g.dart';

@JsonSerializable(explicitToJson: true, createToJson: false)
class MovieSearchResponse extends Equatable {
  const MovieSearchResponse({
    required this.page,
    required this.results,
    required this.totalPages,
    required this.totalResults,
  });

  factory MovieSearchResponse.fromJson(Map<String, dynamic> json) =>
      _$MovieSearchResponseFromJson(json);

  final int page;
  final List<Movie> results;
  @JsonKey(name: 'total_pages')
  final int totalPages;
  @JsonKey(name: 'total_results')
  final int totalResults;

  @override
  List<Object?> get props => [
        page,
        results,
        totalPages,
        totalResults,
      ];
}
