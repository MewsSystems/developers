// ignore_for_file: invalid_annotation_target, depend_on_referenced_packages

import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movie_search/utils/constants.dart';

part 'movie.freezed.dart';
part 'movie.g.dart';

@freezed
class Movie with _$Movie {
  const Movie._();

  factory Movie({
    @JsonKey(name: 'id') required int id,
    @JsonKey(name: 'title') required String title,
    @JsonKey(name: 'overview') @Default('') String overview,
    @JsonKey(name: 'popularity') required double popularity,
    @JsonKey(name: 'adult') required bool isAdult,
    @JsonKey(name: 'video') required bool isVideo,
    @JsonKey(name: 'release_date') String? releaseDate,
    @JsonKey(name: 'poster_path') String? posterPath,
    @JsonKey(name: 'vote_average') required double voteAverage,
    @JsonKey(name: 'vote_count') required int voteCount,
  }) = _Movie;

  String? get imageUrl =>
      posterPath == null ? null : '$IMAGE_ENDPOINT$posterPath';

  factory Movie.fromJson(Map<String, dynamic> json) => _$MovieFromJson(json);
}
