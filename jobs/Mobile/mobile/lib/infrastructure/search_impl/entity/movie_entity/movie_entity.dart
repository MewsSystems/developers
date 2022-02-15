import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';
import 'package:mobile/infrastructure/core/server_address.dart';

part 'movie_entity.freezed.dart';

/// Represent data transfer [Object] for [Movie].
@freezed
class MovieEntity with _$MovieEntity {
  const MovieEntity._();
  const factory MovieEntity({
    required String overview,
    required String title,
    required String originalLanguage,
    required String posterPath,
    required String releaseDate,
    required num voteAverage,
    required int id,
    required num lastPage,
  }) = _MovieEntity;

  factory MovieEntity.fromJson(Map<String, dynamic> json, num lastPage) {
    return MovieEntity(
        id: json["id"],
        overview: json["overview"] ?? '',
        title: json["title"] ?? '',
        posterPath: (json["poster_path"] != null)
            ? const ServerAddress().baseUrl + (json["poster_path"])
            : const ServerAddress().backUpImage,
        originalLanguage: json["original_language"] ?? '',
        releaseDate: json["release_date"] ?? '',
        voteAverage: json["vote_average"] ?? '',
        lastPage: lastPage);
  }

  Movie toModel() {
    return Movie(
        originalLanguage: originalLanguage,
        posterPath: posterPath,
        id: id,
        releaseDate: releaseDate,
        title: title,
        overview: overview,
        lastPage: lastPage,
        voteAverage: voteAverage);
  }
}
