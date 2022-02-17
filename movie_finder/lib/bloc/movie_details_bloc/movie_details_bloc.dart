import 'package:bloc/bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movie_finder/data/model/movies/movie_details_request.dart';
import 'package:movie_finder/data/model/movies/movie_details_result.dart';

import '../../data/movies_service.dart';

part 'movie_details_bloc.freezed.dart';
part 'movie_details_event.dart';
part 'movie_details_state.dart';

class MovieDetailsBloc extends Bloc<MovieDetailsEvent, MovieDetailsState> {
  MovieDetailsBloc()
      : _moviesService = MoviesService(),
        super(MovieDetailsState.initial()) {
    on<FetchRequested>(_onFetchRequested);
  }

  final MoviesService _moviesService;

  Future<void> _onFetchRequested(FetchRequested event, emit) async {
    emit(const MovieDetailsState.loading());
    try {
      final result = await _moviesService.getMovieDetails(
        MovieDetailsRequest(id: event.id),
      );
      emit(
        MovieDetailsState.success(result),
      );
    } catch (e) {
      emit(
        MovieDetailsState.error(e),
      );
    }
  }
}
