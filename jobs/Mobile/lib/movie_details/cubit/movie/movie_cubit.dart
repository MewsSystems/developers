import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:mews_imdb/movie_details/models/models.dart';

import 'package:movie_repository/movie_repository.dart' as rep;

part 'movie_state.dart';

abstract class MovieEvent {}

class MovieCubit extends Cubit<MovieState> {
  MovieCubit({required this.movieId, required this.movieRepository})
      : super(MovieState(movieId: movieId, status: MovieStatus.initial));
  final int movieId;
  final rep.MovieRepository movieRepository;

  void startLoading() async {
    emit(state.copyWith(status: MovieStatus.loading));
    try {
      final rep.MovieDetails result =
          await movieRepository.getMovieDetails(movieId);
      emit(
        state.copyWith(
          status: MovieStatus.success,
          movieDetails: MovieDetails(
            adult: result.adult,
            budget: result.budget,
            genres: result.genres,
            id: result.id,
            title: result.title,
            releaseDate: result.releaseDate,
            voteAverage: result.voteAverage,
            voteCount: result.voteCount,
            posterPath: result.posterPath,
            backdrop: result.backdrop,
            overview: result.overview,
            runtime: result.runtime,
          ),
        ),
      );
    } catch (e) {
      emit(state.copyWith(status: MovieStatus.failure));
    }
  }
}
