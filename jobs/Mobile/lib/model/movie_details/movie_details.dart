import 'package:freezed_annotation/freezed_annotation.dart';

part 'movie_details.freezed.dart';
part 'movie_details.g.dart';

@freezed
class MovieDetails with _$MovieDetails {
  const MovieDetails._();

  const factory MovieDetails({
    required bool adult,
    required int id,
    @JsonKey(name: 'original_title', defaultValue: '') required String originalTitle,
    @JsonKey(name: 'overview', defaultValue: '') required String description,
    @JsonKey(name: 'release_date', defaultValue: '') required String releaseDate,
    //TODO more fields can be added if needed
  }) = _MoviesDetails;

  factory MovieDetails.fromJson(Map<String, dynamic> json) => _$MovieDetailsFromJson(json);
}
