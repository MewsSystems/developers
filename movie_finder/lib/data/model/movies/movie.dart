import 'package:freezed_annotation/freezed_annotation.dart';
part 'movie.freezed.dart';
part 'movie.g.dart';

@freezed
class Movie with _$Movie {
  const factory Movie(
      {int? id,
      String? title,
      @JsonKey(name: 'poster_path') String? posterPath,
      String? overview,
      @JsonKey(name: 'release_date') String? releaseDate}) = _Movie;

  factory Movie.fromJson(Map<String, dynamic> json) => _$MovieFromJson(json);
}
