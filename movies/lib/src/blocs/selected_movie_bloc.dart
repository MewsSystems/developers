import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/model/movie/movie.dart';

abstract class SelectedMovieEvent {}

/// Should be emitted when a movie has been selected
class SelectMovie extends SelectedMovieEvent {
  SelectMovie(this.movie);
  Movie movie;
}

/// Should be emitted when a movie has been selected
class DeselectMovie extends SelectedMovieEvent {}

/// Holds the [Movie] that has been selected, or null when none
class SelectedMovieBloc extends Bloc<SelectedMovieEvent, Movie?> {
  SelectedMovieBloc() : super(null) {
    on<SelectMovie>((event, emit) {
      emit(event.movie);
    });
    on<DeselectMovie>((event, emit) {
      emit(null);
    });
  }
}
