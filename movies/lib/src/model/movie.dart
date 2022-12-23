import 'package:json_annotation/json_annotation.dart';
import 'package:movies/constants.dart';

part 'movie.g.dart';

/// Represents a movie from the search results
@JsonSerializable(fieldRename: FieldRename.snake)
class Movie {
  const Movie({
    required this.id,
    required this.title,
    required this.originalTitle,
    required this.originalLanguage,
    required this.posterPath,
    required this.backdropPath,
    required this.releaseDate,
    required this.voteAverage,
    required this.popularity,
    required this.adult,
    required this.overview,
    required this.genreIds,
  });

  /// Connect the generated [_$MovieFromJson] function to the `fromJson`
  /// factory.
  factory Movie.fromJson(Map<String, dynamic> json) => _$MovieFromJson(json);

  final int id;
  final String title;
  final String originalTitle;
  final String originalLanguage;
  final String? posterPath;
  final String? backdropPath;
  final DateTime releaseDate;
  final double voteAverage;
  final double popularity;
  final bool adult;
  final String overview;
  final List<int> genreIds;

  /// Connect the generated [_$MovieToJson] function to the `toJson` method.
  Map<String, dynamic> toJson() => _$MovieToJson(this);
}

extension ImageUrl on Movie {
  String get originalPoster =>
      posterPath == null ? '' : originalPosterPrefix + (posterPath ?? '');

  String get smallPoster =>
      posterPath == null ? '' : smallPosterPrefix + (posterPath ?? '');

  String get backdrop =>
      backdropPath == null ? '' : originalPosterPrefix + (backdropPath ?? '');
}
