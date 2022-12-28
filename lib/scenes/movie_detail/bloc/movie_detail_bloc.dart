import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';

import '../../home/models/movie.dart';
import '../viewmodel/movie_detail_viewmodel.dart';

part 'movie_detail_bloc.freezed.dart';
part 'movie_detail_event.dart';
part 'movie_detail_state.dart';

class MovieDetailBloc extends Bloc<MovieDetailEvent, MovieDetailState> {
  final Movie movie;
  late final MovieDetailViewModel _viewModel;

  MovieDetailBloc({required this.movie})
      : super(const MovieDetailState.initial()) {
    _viewModel = MovieDetailViewModel(movie);

    on<MovieDetailEvent>((event, emit) {
      event.when(
        started: () => _mapStartedEventToState(),
      );
    });
  }
}

extension MapEventsToStates on MovieDetailBloc {
  void _mapStartedEventToState() {
    emit(MovieDetailState.loadSuccess(viewModel: _viewModel));
  }
}
