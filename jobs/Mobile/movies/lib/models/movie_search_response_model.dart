import 'package:equatable/equatable.dart';
import 'package:movies/models/movie_model.dart';

class MovieSearchResponse extends Equatable {
  const MovieSearchResponse({
    required this.page,
    required this.results,
    required this.totalPages,
    required this.totalResults,
  });

  factory MovieSearchResponse.fromJson(Map<String, dynamic> json) =>
      MovieSearchResponse(
        page: json['page'] as int,
        results: List<Movie>.from(
          (json['results'] as List<dynamic>)
              .map((movie) => Movie.fromJson(movie as Map<String, dynamic>)),
        ),
        totalPages: json['total_pages'] as int,
        totalResults: json['total_results'] as int,
      );

  final int page;
  final List<Movie> results;
  final int totalPages;
  final int totalResults;

  @override
  List<Object?> get props => [
        page,
        results,
        totalPages,
        totalResults,
      ];
}
