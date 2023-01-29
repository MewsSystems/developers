import 'package:movie_app/models/movie_info.dart';

enum MovieInfoLoadStatus {
  loading,
  succeeded,
  failed,
}

class MovieInfoState {
  MovieInfoState({
    required this.movieInfo,
    required this.movieInfoLoadStatus,
  });

  factory MovieInfoState.initial() => MovieInfoState(
        movieInfo: null,
        movieInfoLoadStatus: MovieInfoLoadStatus.loading,
      );

  final MovieInfoLoadStatus movieInfoLoadStatus;
  final MovieInfo? movieInfo;

  MovieInfoState copyWith({
    required MovieInfoLoadStatus movieInfoLoadStatus,
    required MovieInfo? movieInfo,
  }) =>
      MovieInfoState(
        movieInfo: movieInfo,
        movieInfoLoadStatus: movieInfoLoadStatus,
      );
}
