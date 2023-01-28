import 'dart:async';
import 'dart:developer';

import 'package:flutter_bloc/flutter_bloc.dart';

import 'package:movie_app/movie_home/bloc/movies_event.dart';
import 'package:movie_app/movie_home/bloc/movies_state.dart';
import 'package:movie_app/repositories/movies_repository.dart';

class MoviesBloc extends Bloc<MoviesEvent, MoviesState> {
  final TheMovieDbRepository _theMovieDbRepository;
  MoviesBloc({
    required TheMovieDbRepository theMovieDbRepository,
  })  : _theMovieDbRepository = theMovieDbRepository,
        super(MoviesState.initial()) {
    on<MoviesEvent>(
      _onMoviesEvent,
    );
  }

  FutureOr<void> _onMoviesEvent(
    MoviesEvent event,
    Emitter<MoviesState> emit,
  ) async {
    if (event is GetPopularMovies) {
      await _getPopularMovies(emit);
    } else if (event is NextPagePopularMovies) {
      await _getPopularMovies(
        emit,
        page: event.page,
      );
    }
  }

  Future<void> _getPopularMovies(
    Emitter<MoviesState> emit, {
    int page = 1,
  }) async {
    emit(
      state.copyWith(
        moviesLoadStatus: MoviesLoadStatus.loading,
        movieList: state.movieList,
        page: page,
      ),
    );
    try {
      var movies = await _theMovieDbRepository.getPopularMovies(page);
      if (page > 1) {
        movies = state.movieList + movies!;
      }
      emit(
        state.copyWith(
          moviesLoadStatus: MoviesLoadStatus.succeed,
          movieList: movies!,
          page: page,
        ),
      );
    } catch (e) {
      log(e.toString());
      emit(
        state.copyWith(
          moviesLoadStatus: MoviesLoadStatus.failed,
          movieList: [],
          page: page,
        ),
      );
    }
  }
}
