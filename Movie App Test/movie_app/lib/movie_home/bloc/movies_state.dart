import 'package:movie_app/models/movie.dart';

enum MoviesLoadStatus {
  loading,
  succeed,
  failed,
}

class MoviesState {
  final MoviesLoadStatus moviesLoadStatus;
  final List<Movie> movieList;
  final int page;

  MoviesState({
    required this.movieList,
    required this.moviesLoadStatus,
    required this.page,
  });

  factory MoviesState.initial() => MoviesState(
        movieList: [],
        moviesLoadStatus: MoviesLoadStatus.loading,
        page: 1,
      );

  MoviesState copyWith({
    required MoviesLoadStatus moviesLoadStatus,
    required List<Movie> movieList,
    required int page,
  }) =>
      MoviesState(
        movieList: movieList,
        moviesLoadStatus: moviesLoadStatus,
        page: page,
      );
}
