import 'package:freezed_annotation/freezed_annotation.dart';
part 'movie_details_result.freezed.dart';
part 'movie_details_result.g.dart';

@freezed
abstract class MovieDetailsResult with _$MovieDetailsResult {
  const factory MovieDetailsResult({
    String? title,
    String? overview,
    @JsonKey(name: 'original_title') String? originalTitle,
    @JsonKey(name: 'release_date') String? releaseDate,
    @JsonKey(name: 'poster_path') String? posterPath,
    int? revenue,
  }) = _MovieDetailsResult;

  factory MovieDetailsResult.fromJson(Map<String, dynamic> json) => _$MovieDetailsResultFromJson(json);
}
