import 'package:bloc/bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:injectable/injectable.dart';
import 'package:mobile/application/core/enum.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';
import 'package:mobile/domain/search/model/search_params/search_params.dart';
import 'package:mobile/domain/usecases/search.dart';

part 'search_event.dart';
part 'search_state.dart';
part 'search_bloc.freezed.dart';

@injectable
class SearchBloc extends Bloc<SearchEvent, SearchState> {
  final GetSearch getSearch;
  SearchBloc({required this.getSearch}) : super(SearchState.initial()) {
    on<_LoadMore>(_onLoadMore);
    on<_Search>(_onSearch);
  }

  void _onLoadMore(_LoadMore event, Emitter<SearchState> emit) async {
    // emit(state.copyWith(status: Status.loading));
    final res =
        await getSearch(SearchParams(query: event.query, page: event.page));
    res.fold(
        (l) =>
            emit(state.copyWith(status: Status.failed, err: l.cinephileError)),
        (r) => emit(state.copyWith(
            status: Status.success,
            result: r,
            lastPage: r.firstOrNull?.lastPage ?? 0)));
  }

  void _onSearch(_Search event, Emitter<SearchState> emit) async {
    emit(state.copyWith(status: Status.loading));

    final res = await getSearch(SearchParams(query: event.query, page: '1'));
    res.fold(
        (l) =>
            emit(state.copyWith(status: Status.failed, err: l.cinephileError)),
        (r) => emit(state.copyWith(
            status: Status.success,
            result: r,
            lastPage: r.firstOrNull?.lastPage ?? 0)));
  }
}
