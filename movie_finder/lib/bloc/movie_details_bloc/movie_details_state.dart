part of 'movie_details_bloc.dart';

@freezed
class MovieDetailsState with _$MovieDetailsState {
  const factory MovieDetailsState.initial() = MovieDetailsStateInitial;
  const factory MovieDetailsState.loading() = MovieDetailsStateLoading;
  const factory MovieDetailsState.success(MovieDetailsResult movieDetailsResult) = MovieDetailsStateSuccess;
  const factory MovieDetailsState.error(Object error) = MovieDetailsStateError;
}
