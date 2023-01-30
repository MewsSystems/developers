import 'package:equatable/equatable.dart';

abstract class MoviesEvent extends Equatable {
  @override
  List<Object> get props => [];
}

class GetPopularMovies extends MoviesEvent {}

class NextPagePopularMovies extends MoviesEvent {}

class MoviesSearch extends MoviesEvent {
  final String query;

  MoviesSearch({
    required this.query,
  });
  @override
  List<Object> get props => [
        query,
      ];
}
