import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:bloc_concurrency/bloc_concurrency.dart';
import 'package:equatable/equatable.dart';
import 'package:movie_task/movies/movies.dart';
import 'package:stream_transform/stream_transform.dart';

part 'search_event.dart';
part 'search_state.dart';

const throttleDuration = Duration(milliseconds: 100);

EventTransformer<E> throttleDroppable<E>(Duration duration) =>
    (events, mapper) => droppable<E>().call(events.throttle(duration), mapper);

class SearchBloc extends Bloc<SearchEvent, SearchState> {
  SearchBloc({required this.movieRepository}) : super(const SearchState()) {
    on<SearchFetched>(
      _onSearchFetched,
      transformer: throttleDroppable(throttleDuration),
    );
  }

  final MovieRepository movieRepository;
  int currentPage = 1;
  int totalPages = 1;
  String currentQuery = '';

  Future<void> _onSearchFetched(
    SearchFetched event,
    Emitter<SearchState> emit,
  ) async {
    final String query = event.query;
    bool clearList = false;
    if (query.isNotEmpty) {
      if (query.compareTo(currentQuery) != 0) {
        currentQuery = event.query;
        currentPage = 1;
        totalPages = 1;
        clearList = true;
      }
    }
    if (state.hasReachedMax) return;
    try {
      if (currentPage > totalPages) {
        emit(state.copyWith(hasReachedMax: true));
      } else {
        final movies = await movieRepository.getMovies(
          page: currentPage,
          query: currentQuery,
          setPages: _setPages,
        );
        if (clearList) {
          emit(
            state.copyWith(
              status: SearchStatus.success,
              movies: movies,
              hasReachedMax: currentPage > totalPages,
            ),
          );
        } else {
          emit(
            state.copyWith(
              status: SearchStatus.success,
              movies: List.of(state.movies)..addAll(movies),
              hasReachedMax: currentPage > totalPages,
            ),
          );
        }
      }
    } on Exception catch (_) {
      emit(state.copyWith(status: SearchStatus.failure));
    }
  }

  void _setPages(int maxPages) {
    currentPage++;
    totalPages = maxPages;
  }
}
