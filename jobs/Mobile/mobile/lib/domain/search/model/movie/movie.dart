import 'package:freezed_annotation/freezed_annotation.dart';

part 'movie.freezed.dart';

@freezed
class Movie with _$Movie {
  const Movie._();

  const factory Movie(
      {required int id,
      required String posterPath,
      required String originalLanguage,
      required String title,
      required num voteAverage,
      required String releaseDate,
      required String overview,
      required num lastPage}) = _Movie;

  factory Movie.empty() => const Movie(
      id: 0,
      posterPath: '',
      originalLanguage: '',
      title: '',
      voteAverage: 0,
      releaseDate: '',
      overview: '',
      lastPage: 0);
}
