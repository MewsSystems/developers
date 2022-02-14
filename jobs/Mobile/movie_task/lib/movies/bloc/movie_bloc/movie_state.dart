part of 'movie_bloc.dart';

enum MovieStatus { initial, success, failure }

class MovieState extends Equatable {
  const MovieState({
    this.status = MovieStatus.initial,
    this.movie = const DetailedMovie(id: -1, title: '', body: ''),
  });

  final MovieStatus status;
  final DetailedMovie movie;

  MovieState copyWith({
    MovieStatus? status,
    DetailedMovie? movie,
  }) =>
      MovieState(
        status: status ?? this.status,
        movie: movie ?? this.movie,
      );

  @override
  String toString() =>
      '''MovieState { status: $status, movie: ${movie.title},}''';

  @override
  List<Object> get props => [status, movie];
}
