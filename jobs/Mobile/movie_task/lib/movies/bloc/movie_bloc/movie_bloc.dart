import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:bloc_concurrency/bloc_concurrency.dart';
import 'package:equatable/equatable.dart';
import 'package:movie_task/movies/movies.dart';
import 'package:stream_transform/stream_transform.dart';

part 'movie_event.dart';
part 'movie_state.dart';

const throttleDuration = Duration(milliseconds: 100);

EventTransformer<E> throttleDroppable<E>(Duration duration) =>
    (events, mapper) => droppable<E>().call(events.throttle(duration), mapper);

class MovieBloc extends Bloc<MovieEvent, MovieState> {
  MovieBloc({required this.movieRepository}) : super(const MovieState()) {
    on<MovieFetched>(
      _onMovieFetched,
      transformer: throttleDroppable(throttleDuration),
    );
    on<MovieDetailsPopped>(
      _onMovieDetailsPopped,
    );
  }

  final MovieRepository movieRepository;

  Future<void> _onMovieDetailsPopped(
    MovieDetailsPopped event,
    Emitter<MovieState> emit,
  ) async =>
      emit(
        state.copyWith(
          status: MovieStatus.initial,
        ),
      );

  Future<void> _onMovieFetched(
    MovieFetched event,
    Emitter<MovieState> emit,
  ) async {
    try {
      final movieResult =
          await movieRepository.getDetailedMovie(id: event.movie.id);

      return emit(
        state.copyWith(
          status: MovieStatus.success,
          movie: movieResult,
        ),
      );
    } on Exception catch (_) {
      emit(state.copyWith(status: MovieStatus.failure));
    }
  }
}
