part of 'movie_detail_bloc.dart';

@freezed
class MovieDetailState with _$MovieDetailState {
  const factory MovieDetailState.initial() = MovieDetailStateInitial;

  const factory MovieDetailState.loadSuccess({
    required MovieDetailViewModel viewModel,
  }) = MovieDetailStateLoadSuccess;
}
