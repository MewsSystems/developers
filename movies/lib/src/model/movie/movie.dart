import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movies/src/api/datetime_converter.dart';
import 'package:movies/utils.dart';

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
    @DateTimeConverter() DateTime? releaseDate,
    required double voteAverage,
    required double popularity,
    required String overview,
    required List<int> genreIds,
  }) = _Movie;

  factory Movie.fromJson(Map<String, dynamic> json) => _$MovieFromJson(json);
}

extension ImageUrl on Movie {
  // Returns the movie poster in its original size
  String get largePoster =>
      posterPath == null ? '' : getApiImageUrl(posterPath ?? '', 500);

  // Return the movie poster in a small size
  String get smallPoster =>
      posterPath == null ? '' : getApiImageUrl(posterPath ?? '', 342);

  String get backdrop =>
      backdropPath == null ? '' : getApiImageUrl(backdropPath ?? '', 1280);
}
