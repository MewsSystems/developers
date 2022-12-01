part of 'movie_bloc.dart';

abstract class MovieEvent extends Equatable {
  const MovieEvent();

  @override
  List<Object> get props => [];
}

class GetMovieEvent extends MovieEvent {
  const GetMovieEvent(this.id);

  final int id;

  @override
  List<Object> get props => [id];
}
