import 'package:json_annotation/json_annotation.dart';
import 'package:tmdb_api/src/utils/tmdb_utils.dart';

part 'movie_base.g.dart';

@JsonSerializable()
class MovieBase {
  final bool adult;
  @JsonKey(
      fromJson: TMDbUtils.getFullPosterPath,
      toJson: TMDbUtils.getAddedPosterPath)
  final String? backdropPath;
  final int id;
  final String originalLanguage;
  final String originalTitle;
  final num popularity;
  @JsonKey(
      fromJson: TMDbUtils.getFullPosterPath,
      toJson: TMDbUtils.getAddedPosterPath)
  final String? posterPath;
  @JsonKey(fromJson: TMDbUtils.convertToDateTime)
  final DateTime? releaseDate;
  final String title;
  final bool video;
  final num voteAverage;
  final int voteCount;
  MovieBase({
    required this.adult,
    this.backdropPath,
    required this.id,
    required this.originalLanguage,
    required this.originalTitle,
    required this.popularity,
    required this.posterPath,
    this.releaseDate,
    required this.title,
    required this.video,
    required this.voteAverage,
    required this.voteCount,
  });

  factory MovieBase.fromJson(Map<String, dynamic> json) =>
      _$MovieBaseFromJson(json);

  Map<String, dynamic> toJson() => _$MovieBaseToJson(this);
}
