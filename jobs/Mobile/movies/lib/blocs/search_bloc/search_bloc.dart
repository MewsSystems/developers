import 'package:equatable/equatable.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/data/repository/movie_repository.dart';
import 'package:movies/models/movie_model.dart';

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

  int currentPage = 1;
  String query = '';

  Future<void> _onFirstSearchEvent(
    FirstSearchEvent event,
    Emitter<SearchState> emit,
  ) async {
    try {
      if (event.query.trim().isEmpty) {
        emit(
          const ErrorSearchState(message: 'Search request should not be empty'),
        );
      } else {
        currentPage = 1;
        query = event.query;
        emit(LoadingSearchState());
        await _emitMovie(emit);
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
      currentPage++;
      await _emitMovie(emit);
    } on Exception catch (_) {
      emit(const ErrorSearchState(message: 'Ooops, something went wrong'));
    }
  }

  Future<void> _emitMovie(Emitter<SearchState> emit) async {
    final searchResponse = await _movieRepository.getMovies(currentPage, query);
    emit(
      SuccessSearchState(
        currentPage,
        searchResponse.results,
        searchResponse.totalResults,
        searchResponse.page != searchResponse.totalPages,
      ),
    );
  }
}
