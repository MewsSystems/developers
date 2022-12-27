import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/api/movie_search_api.dart';
import 'package:movies/src/blocs/debounce_last.dart';
import 'package:movies/src/model/movie/movie.dart';
import 'package:movies/src/model/movie_search/movie_search_state.dart';

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

class MovieSearchBloc extends Bloc<MovieSearchEvent, MovieSearchState> {
  MovieSearchBloc() : super(const MovieSearchState.result([], true)) {
    on<MovieQueryChanged>(
      (event, emit) async {
        if (event.query.isEmpty) {
          emit(const MovieSearchState.result([], true));
        } else {
          try {
            final movies = await api.searchMovies(event.query);
            emit(MovieSearchState.result(movies, false));
          } on MovieSearchError catch (exception) {
            emit(MovieSearchState.error(exception));
          }
        }
      },
      transformer: debounceLast(const Duration(milliseconds: 300)),
    );
    on<NeedNextMoviePage>(
      (event, emit) async {
        await state.when(
          result: (movies, isLastPage) async {
            if (event.query.isEmpty) {
              emit(const MovieSearchState.result([], true));
            } else {
              try {
                final nextMovies = await api.searchMovies(
                  event.query,
                  page: event.page,
                );
                emit(MovieSearchState.result([...movies, ...nextMovies], false));
              } 
              // Ignore the page error
              on PageError catch(_) {
                emit(MovieSearchState.result(movies, true));
              }
              on MovieSearchError catch (exception) {
                emit(MovieSearchState.error(exception));
              }
            }
          },
          error: (e) {},
        );
      },
      //transformer: debounceLast(const Duration(milliseconds: 300)),
    );
    on<DeleteQuery>((event, emit) {
      emit(const MovieSearchState.result([], true));
    });
  }

  final api = MovieSearchApi();
}
