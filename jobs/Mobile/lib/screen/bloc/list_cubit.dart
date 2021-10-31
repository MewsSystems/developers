import 'package:either_dart/either.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movie_search/api/api_manager.dart';
import 'package:movie_search/model/api_error.dart';
import 'package:movie_search/model/movie_list_item/movie_list_item.dart';
import 'package:movie_search/model/movies_list_response/movies_list_response.dart';

part 'list_cubit.freezed.dart';
part 'list_state.dart';

class ListCubit extends Cubit<ListState> {
  final ApiManager apiManager;
  int _currentPage = 1;
  bool _isLoading = false;
  final List<MovieListItem> _currentItems = [];

  ListCubit(this.apiManager) : super(const ListState.initial());

  void loadNextPage(String input) async {
    if (_isLoading) {
      return;
    }
    _isLoading = true;
    try {
      emit(const ListState.loading());
      Either<ApiError, MoviesListResponse> response = await apiManager.loadMovieList(input, _currentPage);
      response.fold((ApiError error) => emit(ListState.error(_currentItems, error.message ?? '')),
          (MoviesListResponse response) {
        _currentItems.addAll(response.items);
        emit(ListState.loadedData(_currentItems));
        ++_currentPage;
      });
    } on Exception catch (e) {
      emit(ListState.error(_currentItems, 'Data loading error'));
    } finally {
      _isLoading = false;
    }
  }

  void refresh(String input) async {
    _isLoading = false;
    _currentPage = 1;
    _currentItems.clear();
    loadNextPage(input);
  }
}
