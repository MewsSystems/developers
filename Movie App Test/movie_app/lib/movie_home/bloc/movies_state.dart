import 'package:movie_app/models/movie.dart';

enum MoviesLoadStatus {
  loading,
  succeed,
  failed,
}

class MoviesState {
  final MoviesLoadStatus moviesLoadStatus;
  final List<Movie> movieList;
  final bool isFetching;

  MoviesState({
    required this.movieList,
    required this.moviesLoadStatus,
    required this.isFetching,
  });

  factory MoviesState.initial() => MoviesState(
        movieList: [],
        moviesLoadStatus: MoviesLoadStatus.loading,
        isFetching: false,
      );

  MoviesState copyWith({
    required MoviesLoadStatus moviesLoadStatus,
    required List<Movie> movieList,
    required bool isFetching,
  }) =>
      MoviesState(
        movieList: movieList,
        moviesLoadStatus: moviesLoadStatus,
        isFetching: isFetching,
      );
}
