part of 'details_cubit.dart';

@freezed
class DetailsState with _$DetailsState {
  const factory DetailsState.initial() = Initial;

  const factory DetailsState.loading() = Loading;

  const factory DetailsState.loadedData(MovieDetails details) = LoadedData;

  const factory DetailsState.error(String errorMessage) = Error;
}
