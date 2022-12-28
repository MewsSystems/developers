part of 'home_bloc.dart';

@freezed
class HomeState with _$HomeState {
  const factory HomeState.initial({
    @Default(false) bool isListenerState,
  }) = HomeStateInitial;

  const factory HomeState.loading({
    HomeViewModel? viewModel,
    @Default(false) bool isListenerState,
  }) = HomeStateLoading;

  const factory HomeState.loadFailure({
    required Failure failure,
    @Default(false) bool isListenerState,
  }) = HomeStateLoadFailure;

  const factory HomeState.loadSuccess({
    required HomeViewModel viewModel,
    @Default(false) bool isListenerState,
  }) = HomeStateLoadSuccess;

  const factory HomeState.displayAlert({
    required String title,
    required String message,
    @Default(false) bool shouldPopOut,
    @Default(true) bool isListenerState,
  }) = HomeStateDisplayAlert;
}
