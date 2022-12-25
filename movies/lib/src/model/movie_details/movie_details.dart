import 'package:freezed_annotation/freezed_annotation.dart';

part 'movie_details.freezed.dart';

part 'movie_details.g.dart';

/// Represents a movie from the search results
@freezed
class MovieDetails with _$MovieDetails {
  @JsonSerializable(fieldRename: FieldRename.snake)
  const factory MovieDetails({
    required int id,
    required double budget,
    required String homepage,
    String? imdbId,
    required List<Map<String, dynamic>> productionCompanies,
    required List<Map<String, String> >productionCountries,
    required int revenue,
    int? runtime,
    required List<Map<String, String>> spokenLanguages,
    required String status,
    String? tagline,
  }) = _MovieDetails;

  factory MovieDetails.fromJson(Map<String, dynamic> json) => _$MovieDetailsFromJson(json);
}

extension ImdbUrl on MovieDetails {
  String get imdbUrl =>
      imdbId == null ? '' : 'https://www.imdb.com/title/$imdbId/';
}
