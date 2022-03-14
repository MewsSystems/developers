part of 'movie_cubit.dart';

enum MovieStatus { initial, loading, success, failure }

class MovieState extends Equatable {
  MovieState({required this.movieId, required this.status, MovieDetails? movie})
      : movie = movie ?? MovieDetails.empty;

  final MovieStatus status;
  final int movieId;
  final MovieDetails movie;

  MovieState copyWith({
    MovieStatus? status,
    int? movieId,
    MovieDetails? movieDetails,
  }) =>
      MovieState(
        status: status ?? this.status,
        movieId: movieId ?? this.movieId,
        movie: movieDetails ?? this.movie,
      );

  @override
  List<Object> get props => [movieId, status, movie];
}
