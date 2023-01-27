import 'package:movie_app/models/movie.dart';

enum MoviesLoadStatus {
  loading,
  succeed,
  failed,
}

class MoviesState {
  final MoviesLoadStatus moviesLoadStatus;
  final List<Movie> movieList;

  MoviesState({
    required this.movieList,
    required this.moviesLoadStatus,
  });

  factory MoviesState.initial() => MoviesState(
        movieList: [],
        moviesLoadStatus: MoviesLoadStatus.loading,
      );

  MoviesState copyWith({
    required MoviesLoadStatus moviesLoadStatus,
    required List<Movie> movieList,
  }) =>
      MoviesState(
        movieList: movieList,
        moviesLoadStatus: moviesLoadStatus,
      );
}
