import 'dart:async';
import 'dart:developer';

import 'package:flutter_bloc/flutter_bloc.dart';

import 'package:movie_app/movie_home/bloc/movies_event.dart';
import 'package:movie_app/movie_home/bloc/movies_state.dart';
import 'package:movie_app/repositories/movies_repository.dart';

class MoviesBloc extends Bloc<MoviesEvent, MoviesState> {
  final TheMovieDbRepository _theMovieDbRepository;
  int page = 1;
  bool maxPage = false;
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
    } else if (event is CleanSearch) {
      await _getPopularMovies(emit);
    } else if (event is NextPagePopularMovies) {
      await _getNextPage(emit);
    } else if (event is MoviesSearch) {
      await _getSearchMovies(
        emit,
        event.query,
      );
    } else if (event is NextPageSearch) {
      await _getSearchMovies(
        emit,
        state.query!,
      );
    }
  }

  Future<void> _getPopularMovies(
    Emitter<MoviesState> emit,
  ) async {
    if (state.isSearch == true) {
      page = 1;
    }
    emit(
      state.copyWith(
        moviesLoadStatus: MoviesLoadStatus.loading,
        movieList: state.isSearch ? [] : state.movieList,
        isSearch: false,
      ),
    );
    try {
      var movies = await _theMovieDbRepository.getPopularMovies(page);
      if (state.movieList.isNotEmpty) {
        movies = state.movieList + movies!;
      }
      emit(
        state.copyWith(
          moviesLoadStatus: MoviesLoadStatus.succeed,
          movieList: movies!,
          isSearch: false,
        ),
      );
      page++;
    } catch (e) {
      log(e.toString());
      emit(
        state.copyWith(
          moviesLoadStatus: MoviesLoadStatus.failed,
          movieList: [],
          isSearch: false,
        ),
      );
    }
  }

  Future<void> _getNextPage(
    Emitter<MoviesState> emit,
  ) async {
    emit(
      state.copyWith(
        moviesLoadStatus: MoviesLoadStatus.succeed,
        movieList: state.movieList,
        isSearch: false,
      ),
    );
    try {
      var movies = await _theMovieDbRepository.getPopularMovies(page);
      if (state.movieList.isNotEmpty) {
        movies = state.movieList + movies!;
      }
      emit(
        state.copyWith(
          moviesLoadStatus: MoviesLoadStatus.succeed,
          movieList: movies!,
          isSearch: false,
        ),
      );
      page++;
    } catch (e) {
      log(e.toString());
      emit(
        state.copyWith(
          moviesLoadStatus: MoviesLoadStatus.failed,
          movieList: [],
          isSearch: false,
        ),
      );
    }
  }

  Future<void> _getSearchMovies(
    Emitter<MoviesState> emit,
    String query,
  ) async {
    if (state.isSearch == false) {
      page = 1;
    }
    emit(
      state.copyWith(
        moviesLoadStatus: MoviesLoadStatus.loading,
        movieList: state.isSearch ? state.movieList : [],
        isSearch: true,
        query: query,
      ),
    );
    try {
      var movies = await _theMovieDbRepository.searchMovie(query);
      if (state.movieList.isNotEmpty) {
        movies = state.movieList + movies!;
      }
      emit(
        state.copyWith(
          moviesLoadStatus: MoviesLoadStatus.succeed,
          movieList: movies!,
          isSearch: true,
          query: query,
        ),
      );
      page++;
    } catch (e) {
      log(e.toString());
      emit(
        state.copyWith(
          moviesLoadStatus: MoviesLoadStatus.failed,
          movieList: [],
          isSearch: true,
        ),
      );
    }
  }
}
