import 'package:movie_app/models/movie.dart';

enum MoviesLoadStatus {
  loading,
  succeed,
  failed,
}

class MoviesState {
  final MoviesLoadStatus moviesLoadStatus;
  final List<Movie> movieList;
  final bool isSearch;
  final String? query;

  MoviesState({
    required this.movieList,
    required this.moviesLoadStatus,
    required this.isSearch,
    this.query,
  });

  factory MoviesState.initial() => MoviesState(
        movieList: [],
        moviesLoadStatus: MoviesLoadStatus.loading,
        isSearch: false,
        query: null,
      );

  MoviesState copyWith({
    required MoviesLoadStatus moviesLoadStatus,
    required List<Movie> movieList,
    required bool isSearch,
    String? query,
  }) =>
      MoviesState(
        movieList: movieList,
        moviesLoadStatus: moviesLoadStatus,
        isSearch: isSearch,
        query: query,
      );
}
