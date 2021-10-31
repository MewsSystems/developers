import 'package:either_dart/either.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movie_search/api/api_manager.dart';
import 'package:movie_search/model/api_error/api_error.dart';
import 'package:movie_search/model/movie_list_item/movie_list_item.dart';
import 'package:movie_search/model/movies_list_response/movies_list_response.dart';

part 'list_cubit.freezed.dart';
part 'list_state.dart';

class ListCubit extends Cubit<ListState> {
  final ApiManager apiManager;
  int _currentPage = 1;
  String _currentInput = '';
  int _totalPages = -1;

  ListCubit(this.apiManager) : super(const ListState.initial());

  Future<void> loadNextPage(String input) async {
    if (_totalPages >= 0 && _currentPage > _totalPages) {
      emit(ListState.allDataLoaded(items: state.items));
      return;
    }
    try {
      emit(ListState.loading(items: state.items));
      Either<ApiError, MoviesListResponse> response = await apiManager.loadMovieList(input, _currentPage);
      if (input != _currentInput) {
        return;
      }
      response.fold((ApiError error) => emit(ListState.error(items: state.items, errorMessage: error.message ?? '')),
          (MoviesListResponse response) {
        debugPrint('Loaded page ${response.page}');
        debugPrint("Total pages ${response.totalPages}");
        List<MovieListItem> currentItems = List.from(state.items)..addAll(response.items);
        _totalPages = response.totalPages;
        emit(ListState.loadedData(items: currentItems, pageNumber: response.page));
        ++_currentPage;
      });
    } on Exception catch (e) {
      emit(ListState.error(items: state.items, errorMessage: 'Data loading error'));
    }
  }

  Future<void> refresh(String input) async {
    if (_currentInput == input) {
      return;
    }
    debugPrint('Into Refresh');
    emit(const ListState.loading(items: []));
    _currentInput = input;
    _currentPage = 1;
    _totalPages = -1;
    loadNextPage(input);
  }
}
