import 'package:equatable/equatable.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/models/detailed_movie_model.dart';
import 'package:movies/networking/repository/movie_repository.dart';

part 'movie_event.dart';
part 'movie_state.dart';

class MovieBloc extends Bloc<MovieEvent, MovieState> {
  MovieBloc({required MovieRepository movieRepository})
      : _movieRepository = movieRepository,
        super(InitialMovieState()) {
    on<GetMovieEvent>(_onGetMovieEvent);
  }

  final MovieRepository _movieRepository;

  Future<void> _onGetMovieEvent(
    GetMovieEvent event,
    Emitter<MovieState> emit,
  ) async {
    try {
      emit(LoadingMovieState());
      emit(SuccessMovieState(await _movieRepository.getMovieById(event.id)));
    } on Exception catch (_) {
      emit(const ErrorMovieState(message: 'Ooops, something went wrong'));
    }
  }
}
