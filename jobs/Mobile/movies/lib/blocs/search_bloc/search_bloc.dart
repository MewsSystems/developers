import 'package:equatable/equatable.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/core/errors/exceptions.dart';
import 'package:movies/core/errors/network_exceptions.dart';
import 'package:movies/models/movie_model.dart';
import 'package:movies/networking/repository/movie_repository.dart';

part 'search_event.dart';
part 'search_state.dart';

class SearchBloc extends Bloc<SearchEvent, SearchState> {
  SearchBloc({required MovieRepository movieRepository})
      : _movieRepository = movieRepository,
        super(InitialSearchState()) {
    on<FirstSearchEvent>(_onFirstSearchEvent);
    on<NextSearchEvent>(_onGetNextSearchEvent);
  }

  final MovieRepository _movieRepository;

  Future<void> _onFirstSearchEvent(
    FirstSearchEvent event,
    Emitter<SearchState> emit,
  ) async {
    final query = event.query.trim();
    if (query.isEmpty) {
      emit(
        const ErrorSearchState(message: 'Search request should not be empty'),
      );
    } else {
      emit(LoadingSearchState());
      await _emitMovie(emit, 1, query);
    }
  }

  Future<void> _onGetNextSearchEvent(
    NextSearchEvent _,
    Emitter<SearchState> emit,
  ) async {
    if (state is SuccessSearchState) {
      final currentState = state as SuccessSearchState;
      final currentPage = currentState.page + 1;
      final currentQuery = currentState.query;
      await _emitMovie(emit, currentPage, currentQuery);
    } else {
      emit(const ErrorSearchState(message: 'Ooops, something went wrong'));
    }
  }

  Future<void> _emitMovie(
    Emitter<SearchState> emit,
    int page,
    String query,
  ) async {
    final failureOrMovie = await _movieRepository.getMovies(page, query);
    failureOrMovie.fold(
      (failure) => emit(
        ErrorSearchState(message: _mapFailureToMessage(failure)),
      ),
      (searchResponse) => emit(
        SuccessSearchState(
          page,
          query,
          searchResponse.results,
          searchResponse.totalResults,
          searchResponse.page != searchResponse.totalPages,
        ),
      ),
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
