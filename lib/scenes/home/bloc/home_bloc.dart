import 'dart:async';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
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
  final HomeRepository repository;
  late final StreamSubscription _subscriptionSearch;
  HomeViewModel _viewModel = HomeViewModel(movies: []);

  SearchMoviesResponse? _response;

  HomeBloc({required this.repository}) : super(const HomeState.initial()) {
    _subscriptionSearch = _searchMovieController
        .debounceTime(const Duration(milliseconds: 500))
        .listen((searchText) {
      add(HomeEvent.searchMovies(searchText));
    });

    on<HomeEvent>((event, emit) {
      event.when(
        didChangeSearch: (searchText) =>
            _searchMovieController.sink.add(searchText),
        searchMovies: (searchText) => _mapSearchMoviesEventToState(searchText),
      );
    });
  }

  @override
  Future<void> close() {
    _subscriptionSearch.cancel();
    _searchMovieController.close();
    return super.close();
  }
}

extension MapEventsToStates on HomeBloc {
  void _mapSearchMoviesEventToState(String searchText) async {
    emit(HomeState.loading(viewModel: _viewModel));

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
      },
      (searchResponse) {
        _viewModel = _viewModel.copyWith(movies: searchResponse.movies);
      },
    );
  }
}
