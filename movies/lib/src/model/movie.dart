import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movies/constants.dart';
import 'package:movies/src/api/datetime_converter.dart';

part 'movie.freezed.dart';

part 'movie.g.dart';

/// Represents a movie from the search results
@freezed
class Movie with _$Movie {
  @JsonSerializable(fieldRename: FieldRename.snake)
  const factory Movie({
    required int id,
    required String title,
    required String originalTitle,
    required String originalLanguage,
    String? posterPath,
    String? backdropPath,
    @DateTimeConverter()
    DateTime? releaseDate,
    required double voteAverage,
    required double popularity,
    required String overview,
    required List<int> genreIds,
  }) = _Movie;

  /// Connect the generated [_$MovieFromJson] function to the `fromJson`
  /// factory.
  factory Movie.fromJson(Map<String, dynamic> json) => _$MovieFromJson(json);

  /// Connect the generated [_$MovieToJson] function to the `toJson` method.
  //Map<String, dynamic> toJson() => _$MovieToJson(this);
}

extension ImageUrl on Movie {
  String get originalPoster =>
      posterPath == null ? '' : originalPosterPrefix + (posterPath ?? '');

  String get smallPoster =>
      posterPath == null ? '' : smallPosterPrefix + (posterPath ?? '');

  String get backdrop =>
      backdropPath == null ? '' : originalPosterPrefix + (backdropPath ?? '');
}
