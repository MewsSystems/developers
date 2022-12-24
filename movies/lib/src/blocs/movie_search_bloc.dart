import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/api/movie_search_api.dart';
import 'package:movies/src/model/movie.dart';

abstract class MovieSearchEvent {}

class MovieQueryChanged extends MovieSearchEvent {
  MovieQueryChanged(this.query);
  String query;
}

class DeleteQuery extends MovieSearchEvent {}

class MovieSearchBloc extends Bloc<MovieSearchEvent, List<Movie>> {
  MovieSearchBloc() : super([]) {
    on<MovieQueryChanged>((event, emit) async {
      final movies = await client.searchMovies(event.query);
      emit(movies);
    });
    on<DeleteQuery>((event, emit) {
      emit([]);
    });
  }
  final client = MovieSearchApi();
}
