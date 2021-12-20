import 'package:equatable/equatable.dart';
import 'package:move_app/data/repositories/models.dart'

;enum MoviesStatus{initial, loading, success, failure}

class MoviesState extends Equatable {

  final MoviesStatus status;
  final List<Movie> movies;

  const MoviesState({
    this.status = MoviesStatus.initial,
    List<Movie>? movies}) : 
    movies = movies ?? const[];

  MoviesState copyWith({
    MoviesStatus? status,
    List<Movie>? moviesPreview
  }) {
    return MoviesState(
      status: status ?? this.status,
      movies: moviesPreview ?? movies
    );
  }

  @override
  List<Object?> get props => [status, movies];
}