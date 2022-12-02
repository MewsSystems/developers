part of 'movie_bloc.dart';

abstract class MovieState extends Equatable {
  const MovieState();

  @override
  List<Object> get props => [];
}

class InitialMovieState extends MovieState {}

class LoadingMovieState extends MovieState {}

class SuccessMovieState extends MovieState {
  const SuccessMovieState(this.movie);

  final DetailedMovie movie;

  @override
  List<Object> get props => [movie];
}

class ErrorMovieState extends MovieState {
  const ErrorMovieState({required this.message});

  final String message;

  @override
  List<Object> get props => [message];
}
