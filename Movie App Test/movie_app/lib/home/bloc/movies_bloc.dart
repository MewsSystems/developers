import 'dart:async';
import 'dart:developer';

import 'package:flutter_bloc/flutter_bloc.dart';

import 'package:movie_app/home/bloc/movies_event.dart';
import 'package:movie_app/home/bloc/movies_state.dart';
import 'package:movie_app/repositories/movies_repository.dart';

class MoviesBloc extends Bloc<MoviesEvent, MoviesState> {
  final TheMovieDbRepository _theMovieDbRepository;
  MoviesBloc({
    required TheMovieDbRepository theMovieDbRepository,
  })  : _theMovieDbRepository = theMovieDbRepository,
        super(MoviesState.initial()) {
    on<MoviesEvent>(
      _onCoinsEvent,
    );
  }

  FutureOr<void> _onCoinsEvent(
    MoviesEvent event,
    Emitter<MoviesState> emit,
  ) async {
    if (event is GetPopularMovies) {
      await _getPopularMovies(emit);
    }
  }

  Future<void> _getPopularMovies(
    Emitter<MoviesState> emit,
  ) async {
    emit(
      state.copyWith(
        moviesLoadStatus: MoviesLoadStatus.loading,
        movieList: [],
      ),
    );
    try {
      var movies = await _theMovieDbRepository.getPopularMovies();
      emit(
        state.copyWith(
          moviesLoadStatus: MoviesLoadStatus.succeed,
          movieList: movies!,
        ),
      );
    } catch (e) {
      log(e.toString());
      emit(
        state.copyWith(
          moviesLoadStatus: MoviesLoadStatus.failed,
          movieList: [],
        ),
      );
    }
  }
}
