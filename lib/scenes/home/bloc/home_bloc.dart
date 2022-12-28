// ignore_for_file: invalid_use_of_visible_for_testing_member

import 'dart:async';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movie_search/scenes/home/models/movie.dart';
import 'package:rxdart/rxdart.dart';

import '../../../common/models/failure.dart';
import '../models/search_movies_request.dart';
import '../models/search_movies_response.dart';
import '../repository/home_repository.dart';
import '../viewmodel/home_viewmodel.dart';

part 'home_bloc.freezed.dart';
part 'home_event.dart';
part 'home_state.dart';

class HomeBloc extends Bloc<HomeEvent, HomeState> {
  final PublishSubject<String> _searchMovieController = PublishSubject();
  final PublishSubject _autoFetchController = PublishSubject();

  final HomeRepository repository;
  late final StreamSubscription _subscriptionSearch;
  late final StreamSubscription _subscriptionFetcher;
  HomeViewModel _viewModel = const HomeViewModel(movies: []);

  SearchMoviesResponse? _response;
  String _searchPhrase = '';

  bool get hasMoreRecords {
    if (_response == null) return false;

    if (_response!.totalPages == _response!.page) return false;

    return true;
  }

  bool _isFetchingRecords = false;
  bool get isFetchingRecords => _isFetchingRecords;

  HomeBloc({required this.repository}) : super(const HomeState.initial()) {
    _subscriptionSearch = _searchMovieController
        .debounceTime(const Duration(milliseconds: 500))
        .listen((searchText) {
      if (searchText.trim().isNotEmpty) {
        add(HomeEvent.searchMovies(searchText));
      } else {
        add(const HomeEvent.clearList());
      }
    });

    _subscriptionFetcher = _autoFetchController
        .debounceTime(const Duration(milliseconds: 300))
        .listen((event) {
      if (!_isFetchingRecords && hasMoreRecords) {
        add(const HomeEvent.fetchMoreRecords());
      }
    });

    on<HomeEvent>((event, emit) {
      event.when(
        didChangeSearch: (searchText) =>
            _searchMovieController.sink.add(searchText),
        searchMovies: (searchText) => _mapSearchMoviesEventToState(searchText),
        clearList: () => _mapClearListEventToState(),
        retrySearch: (searchText) =>
            _searchMovieController.sink.add(searchText),
        requestMoreRecords: () => _autoFetchController.sink.add(true),
        fetchMoreRecords: () => _mapFetchMoreRecordsEventToState(),
      );
    });
  }

  @override
  Future<void> close() {
    _subscriptionFetcher.cancel();
    _autoFetchController.close();
    _subscriptionSearch.cancel();
    _searchMovieController.close();
    return super.close();
  }
}

extension MapEventsToStates on HomeBloc {
  void _mapFetchMoreRecordsEventToState() async {
    if (_response == null || isFetchingRecords) return;

    _isFetchingRecords = true;

    final request = SearchMoviesRequest(
        searchText: _searchPhrase, page: _response!.nextPage);

    final response = await repository.searchMovies(request: request);

    response.fold(
      (failure) {
        emit(const HomeState.displayAlert(
            title: "Fetch more records\nfailed",
            message:
                "We couldn't fetch more records. Please check your internet connection and try again."));
      },
      (searchResponse) {
        _response = searchResponse;
        List<Movie> updatedMovies = [..._viewModel.movies];
        updatedMovies.addAll(searchResponse.movies);

        _viewModel = _viewModel.copyWith(movies: updatedMovies);
      },
    );
    emit(HomeState.loadSuccess(viewModel: _viewModel));
    _isFetchingRecords = false;
  }

  void _mapClearListEventToState() {
    _viewModel = _viewModel.copyWith(movies: []);
    _response = null;
    emit(HomeState.loadSuccess(viewModel: _viewModel));
  }

  void _mapSearchMoviesEventToState(String searchText) async {
    emit(HomeState.loading(viewModel: _viewModel));

    _searchPhrase = searchText;

    final request = SearchMoviesRequest(
        searchText: searchText, page: _response?.nextPage ?? 1);

    final response = await repository.searchMovies(request: request);

    response.fold(
      (failure) {
        if (failure is ConnectionFailure) {
          emit(HomeState.loadFailure(failure: failure));
          return;
        }
        emit(
            HomeState.displayAlert(title: "Failure", message: failure.message));
        emit(HomeState.loadSuccess(viewModel: _viewModel));
      },
      (searchResponse) {
        _response = searchResponse;
        _viewModel = _viewModel.copyWith(movies: searchResponse.movies);
        emit(HomeState.loadSuccess(viewModel: _viewModel));
      },
    );
  }
}
