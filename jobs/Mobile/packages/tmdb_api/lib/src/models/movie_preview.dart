import 'package:json_annotation/json_annotation.dart';
import 'package:tmdb_api/src/models/movie_base.dart';
import 'package:tmdb_api/src/utils/tmdb_utils.dart';

part 'movie_preview.g.dart';

@JsonSerializable()
class MoviePreview extends MovieBase {
  final String overview;
  final List<int> genreIds;

  MoviePreview({
    required adult,
    backdropPath,
    required id,
    required originalLanguage,
    required originalTitle,
    required popularity,
    required posterPath,
    releaseDate,
    required title,
    required video,
    required voteAverage,
    required voteCount,
    required this.overview,
    required this.genreIds,
  }) : super(
            adult: adult,
            backdropPath: backdropPath,
            id: id,
            originalLanguage: originalLanguage,
            originalTitle: originalTitle,
            popularity: popularity,
            posterPath: posterPath,
            releaseDate: releaseDate,
            title: title,
            video: video,
            voteAverage: voteAverage,
            voteCount: voteCount);

  factory MoviePreview.fromJson(Map<String, dynamic> json) =>
      _$MoviePreviewFromJson(json);

  Map<String, dynamic> toJson() => _$MoviePreviewToJson(this);
}
