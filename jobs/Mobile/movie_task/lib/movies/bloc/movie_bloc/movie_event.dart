part of 'movie_bloc.dart';

abstract class MovieEvent extends Equatable {
  @override
  List<Object> get props => [];
}

class MovieFetched extends MovieEvent {
  MovieFetched(this.movie);

  final Movie movie;
}

class MovieDetailsPopped extends MovieEvent {}
