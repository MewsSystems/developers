import 'package:equatable/equatable.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
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
    try {
      final query = event.query.trim();
      if (query.isEmpty) {
        emit(
          const ErrorSearchState(message: 'Search request should not be empty'),
        );
      } else {
        emit(LoadingSearchState());
        await _emitMovie(emit, 1, query);
      }
    } on Exception catch (_) {
      emit(const ErrorSearchState(message: 'Ooops, something went wrong'));
    }
  }

  Future<void> _onGetNextSearchEvent(
    NextSearchEvent _,
    Emitter<SearchState> emit,
  ) async {
    try {
      if (state is SuccessSearchState) {
        final currentState = state as SuccessSearchState;
        final currentPage = currentState.page + 1;
        final currentQuery = currentState.query;
        await _emitMovie(emit, currentPage, currentQuery);
      } else {
        emit(const ErrorSearchState(message: 'Ooops, something went wrong'));
      }
    } on Exception catch (_) {
      emit(const ErrorSearchState(message: 'Ooops, something went wrong'));
    }
  }

  Future<void> _emitMovie(
    Emitter<SearchState> emit,
    int page,
    String query,
  ) async {
    final searchResponse = await _movieRepository.getMovies(page, query);
    emit(
      SuccessSearchState(
        page,
        query,
        searchResponse.results,
        searchResponse.totalResults,
        searchResponse.page != searchResponse.totalPages,
      ),
    );
  }
}
