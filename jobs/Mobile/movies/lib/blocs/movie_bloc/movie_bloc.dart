import 'package:equatable/equatable.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/core/errors/exceptions.dart';
import 'package:movies/core/errors/network_exceptions.dart';
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
    emit(LoadingMovieState());
    final failureOrMovie = await _movieRepository.getMovieById(event.id);
    failureOrMovie.fold(
      (failure) => emit(
        ErrorMovieState(message: _mapFailureToMessage(failure)),
      ),
      (movie) => emit(SuccessMovieState(movie)),
    );
  }

  String _mapFailureToMessage(Failure failure) {
    switch (failure.runtimeType) {
      case NetworkFailure:
        final networkException = (failure as NetworkFailure).exception;
        final message = NetworkExceptions.getErrorMessage(networkException);
        return message;
      default:
        return 'Unexpected error';
    }
  }
}
