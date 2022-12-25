import 'dart:async';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/api/movie_details_api.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/model/movie/movie.dart';
import 'package:movies/src/model/movie_details/movie_details_state.dart';

class MovieDetailsCubit extends Cubit<MovieDetailsState> {
  MovieDetailsCubit(this.selectedMovieBloc)
      : super(const MovieDetailsState.noSelection()) {
    selectedMovieSubscription = selectedMovieBloc.stream.listen((state) async {
      if (state == null) {
        emit(const MovieDetailsState.noSelection());
      } else {
        try {
          emit(const MovieDetailsState.loading());
          final details = await api.getMovieDetails(state.id);
          emit(MovieDetailsState.details(details));
        } on MovieDetailsError catch (e) {
          emit(MovieDetailsState.error(e));
        }
      }
    });
  }
  final SelectedMovieBloc selectedMovieBloc;
  late StreamSubscription<Movie?> selectedMovieSubscription;
  final api = MovieDetailsApi();

  @override
  Future<void> close() {
    selectedMovieSubscription.cancel();

    return super.close();
  }
}
