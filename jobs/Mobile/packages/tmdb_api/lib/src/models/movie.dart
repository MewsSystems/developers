import 'package:json_annotation/json_annotation.dart';
import 'package:tmdb_api/src/models/models.dart';
import 'package:tmdb_api/src/models/movie_base.dart';
import 'package:tmdb_api/src/utils/tmdb_utils.dart';

part 'movie.g.dart';

@JsonSerializable()
class Movie extends MovieBase {
  final int budget;
  final List<Genre> genres;
  final String? homepage;
  final String? imdbId;
  final String? overview;
  final List<Company> productionCompanies;
  final List<Country> productionCountries;
  final int revenue;
  final int? runtime;
  final List<Language> spokenLanguages;
  final String status;
  final String? tagline;

  Movie({
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
    required this.budget,
    required this.genres,
    required this.homepage,
    required this.imdbId,
    required this.overview,
    required this.productionCompanies,
    required this.productionCountries,
    required this.revenue,
    required this.runtime,
    required this.spokenLanguages,
    required this.status,
    required this.tagline,
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

  factory Movie.fromJson(Map<String, dynamic> json) => _$MovieFromJson(json);

  Map<String, dynamic> toJson() => _$MovieToJson(this);
}
