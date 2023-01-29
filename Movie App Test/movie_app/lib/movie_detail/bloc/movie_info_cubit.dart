// Copyright (c) 2022, Very Good Ventures
// https://verygood.ventures
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

import 'dart:convert';
import 'dart:developer';

import 'package:bloc/bloc.dart';
import 'package:movie_app/movie_detail/bloc/movie_info_state.dart';
import 'package:movie_app/repositories/movies_repository.dart';

class MovieInfoCubit extends Cubit<MovieInfoState> {
  final TheMovieDbRepository _theMovieDbRepository;

  MovieInfoCubit({
    required TheMovieDbRepository theMovieDbRepository,
  })  : _theMovieDbRepository = theMovieDbRepository,
        super(
          MovieInfoState.initial(),
        );

  Future<void> getMovieInfo(int movieId) async {
    emit(
      state.copyWith(
        movieInfo: null,
        movieInfoLoadStatus: MovieInfoLoadStatus.loading,
      ),
    );
    try {
      final coffee = await _theMovieDbRepository.getMovieInfo(movieId);
      emit(
        state.copyWith(
          movieInfo: coffee,
          movieInfoLoadStatus: MovieInfoLoadStatus.succeeded,
        ),
      );
    } catch (e) {
      log(e.toString());
      emit(
        state.copyWith(
          movieInfo: null,
          movieInfoLoadStatus: MovieInfoLoadStatus.failed,
        ),
      );
    }
  }
}
