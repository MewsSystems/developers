import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:mews_imdb/search/models/models.dart';
import 'package:movie_repository/movie_repository.dart' as rep;

part 'search_event.dart';
part 'search_state.dart';

enum SearchStatus { initial, loading, success, failure }

class SearchBloc extends Bloc<SearchEvent, SearchState> {
  SearchBloc({required this.movieRepository}) : super(const SearchState()) {
    on<NewSearchEvent>(_onSearchRequested);
    on<NextPageSearchEvent>(_onLoadNext);
    on<ResetSearchEvent>(_onReset);
  }

  final rep.MovieRepository movieRepository;

  Future<void> _onSearchRequested(
    SearchEvent event,
    Emitter<SearchState> emit,
  ) async {
    try {
      emit(state.copyWith(status: SearchStatus.loading));
      final rep.SearchResult searchResult =
          await movieRepository.search(event.query);
      final List<MoviePreview> moviePreviews = searchResult.previews
          .map(
            (preview) => MoviePreview(
              id: preview.id,
              title: preview.title,
              voteAverage: preview.voteAverage,
              releaseDate: preview.releaseDate,
              overview: preview.overview,
              posterPath: preview.posterPath,
            ),
          )
          .toList();
      emit(
        state.copyWith(
          status: SearchStatus.success,
          previews: moviePreviews,
          query: event.query,
          page: event.page,
          maxPage: searchResult.totalPages,
          hasReachedMax: false,
        ),
      );
    } catch (_) {
      emit(state.copyWith(status: SearchStatus.failure));
    }
  }

  FutureOr<void> _onLoadNext(
    NextPageSearchEvent event,
    Emitter<SearchState> emit,
  ) async {
    try {
      if (state.hasReachedMax) return;
      final rep.SearchResult nextResult =
          await movieRepository.search(event.query, event.page);
      final List<MoviePreview> moviePreviews = nextResult.previews
          .map(
            (preview) => MoviePreview(
              id: preview.id,
              title: preview.title,
              voteAverage: preview.voteAverage,
              releaseDate: preview.releaseDate,
              overview: preview.overview,
              posterPath: preview.posterPath,
            ),
          )
          .toList();
      emit(
        state.copyWith(
          status: SearchStatus.success,
          previews: List.of(state.previews)..addAll(moviePreviews),
          page: event.page,
          hasReachedMax: event.page == nextResult.totalPages,
        ),
      );
    } catch (_) {
      emit(state.copyWith(status: SearchStatus.failure));
    }
  }

  FutureOr<void> _onReset(ResetSearchEvent event, Emitter<SearchState> emit) {
    emit(
      state.copyWith(
        status: SearchStatus.initial,
        query: '',
        maxPage: 1,
        page: 1,
        previews: [],
        hasReachedMax: false,
      ),
    );
  }
}
