import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/api/movie_search_api.dart';
import 'package:movies/src/blocs/debounce_last.dart';
import 'package:movies/src/model/movie.dart';

abstract class MovieSearchEvent {}

class MovieQueryChanged extends MovieSearchEvent {
  MovieQueryChanged(this.query);
  String query;
}

class NeedNextMoviePage extends MovieSearchEvent {
  NeedNextMoviePage(this.query, this.page);
  String query;
  int page;
}

class DeleteQuery extends MovieSearchEvent {}

class MovieSearchBloc extends Bloc<MovieSearchEvent, List<Movie>> {
  MovieSearchBloc() : super([]) {
    on<MovieQueryChanged>(
      (event, emit) async {
        if (event.query.isEmpty) {
          emit([]);
        } else {
          final movies = await client.searchMovies(event.query);
          emit(movies);
        }
      },
      transformer: debounceLast(const Duration(milliseconds: 300)),
    );
    on<NeedNextMoviePage>(
      (event, emit) async {
        final movies = await client.searchMovies(
          event.query,
          page: event.page,
        );
        emit([
          ...state,
          ...movies
        ]);
      },
      //transformer: debounceLast(const Duration(milliseconds: 300)),
    );
    on<DeleteQuery>((event, emit) {
      emit([]);
    });
  }
  final client = MovieSearchApi();
}
