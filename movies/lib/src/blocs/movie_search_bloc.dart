import 'dart:async';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/api/movie_search_api.dart';
import 'package:movies/src/blocs/debounce_last.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/model/movie/movie.dart';
import 'package:movies/src/model/movie_search/movie_search_state.dart';

abstract class MovieSearchEvent {}

class QueryChanged extends MovieSearchEvent {
  QueryChanged(this.query);
  String query;
}

class NeedNextMoviePage extends MovieSearchEvent {
  NeedNextMoviePage(this.query, this.page);
  String query;
  int page;
}

class DeleteQuery extends MovieSearchEvent {}

class MovieSearchBloc extends Bloc<MovieSearchEvent, MovieSearchState> {
  MovieSearchBloc() : super(MovieSearchState.emptyResult) {
    on<QueryChanged>(
      (event, emit) async {
        if (event.query.isEmpty) {
          emit(MovieSearchState.emptyResult);
        } else {
          try {
            final movies = await api.searchMovies(event.query);
            emit(MovieSearchState.result(event.query, movies, false));
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
          result: (query, movies, isLastPage) async {
            if (event.query.isEmpty) {
              emit(MovieSearchState.emptyResult);
            } else {
              try {
                final nextMovies = await api.searchMovies(
                  event.query,
                  page: event.page,
                );
                emit(MovieSearchState.result(
                    event.query, [...movies, ...nextMovies], false));
              }
              // Ignore the page error
              on PageError catch (_) {
                emit(MovieSearchState.result(event.query, movies, true));
              } on MovieSearchError catch (exception) {
                emit(MovieSearchState.error(exception));
              }
            }
          },
          error: (e) {},
        );
      },
      transformer: debounceLast(const Duration(milliseconds: 300)),
    );
    on<DeleteQuery>((event, emit) {
      emit(MovieSearchState.emptyResult);
    });
  }

  final api = MovieSearchApi();
}
