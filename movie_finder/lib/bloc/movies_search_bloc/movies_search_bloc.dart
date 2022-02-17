import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movie_finder/data/model/movies/movie.dart';
import 'package:movie_finder/data/model/movies/movies_search_request.dart';
import 'package:movie_finder/data/movies_service.dart';
import 'package:rxdart/rxdart.dart';

part 'movies_search_event.dart';
part 'movies_search_state.dart';
part 'movies_search_bloc.freezed.dart';

class MoviesSearchBloc extends Bloc<MoviesSearchEvent, MoviesSearchState> {
  MoviesSearchBloc()
      : _moviesService = MoviesService(),
        _subscription = CompositeSubscription(),
        super(const MoviesSearchState.initial()) {
    on<FetchRequested>(_onFetchRequested);

    movieQuery.debounceTime(const Duration(milliseconds: 500)).listen((value) {
      _setInitialValues();
      if (value.isNotEmpty) {
        requestFetch();
      } else {
        emit(MoviesSearchState.initial());
      }
    }).addTo(_subscription);
  }

  final CompositeSubscription _subscription;
  final MoviesService _moviesService;
  final page = BehaviorSubject<int>.seeded(1);
  final movies = BehaviorSubject<List<Movie>>.seeded(<Movie>[]);
  final movieQuery = BehaviorSubject<String>.seeded('');

  Future<void> _onFetchRequested(FetchRequested event, emit) async {
    emit(const MoviesSearchState.loading());
    try {
      final result = await _moviesService.searchMovies(
        MoviesSearchRequest(
          query: movieQuery.value,
          page: page.value,
        ),
      );

      if (result.results?.isEmpty == true && movies.value.isNotEmpty) {
        emit(
          MoviesSearchState.noMorePages(movies.value),
        );
      } else {
        movies.add(movies.value..addAll(result.results ?? <Movie>[]));
        emit(
          MoviesSearchState.success(movies.value),
        );
      }
    } catch (e) {
      emit(
        MoviesSearchState.error(e),
      );
    }
  }

  void requestFetch() => add(const MoviesSearchEvent.fetchRequested());

  void _setInitialValues() {
    page.add(1);
    movies.add(<Movie>[]);
  }

  void incrementPage() {
    page.add(page.value + 1);
  }

  @override
  Future<void> close() {
    _subscription.dispose();
    return super.close();
  }
}
