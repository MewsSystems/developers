import 'package:equatable/equatable.dart';

abstract class MoviesEvent extends Equatable {
  @override
  List<Object> get props => [];
}

class GetPopularMovies extends MoviesEvent {}

class MoviesSearch extends MoviesEvent {
  final String movieName;

  MoviesSearch({
    required this.movieName,
  });
  @override
  List<Object> get props => [
        movieName,
      ];
}
