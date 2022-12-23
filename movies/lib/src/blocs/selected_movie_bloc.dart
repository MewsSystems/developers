import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/pages/movie.dart';

abstract class SelectedMovieEvent {}

class SelectMovie extends SelectedMovieEvent {
  SelectMovie(this.movie);
  Movie movie;
}
class DeselectMovie extends SelectedMovieEvent {}

class SelectedMovieBloc extends Bloc<SelectedMovieEvent, Movie?> {
  SelectedMovieBloc() : super(null){
    on<SelectMovie>((event, emit) {
      emit(event.movie);
    });
    super.on<DeselectMovie>((event, emit) {
      emit(null);
    });
  }

   @override
  void onChange(Change<Movie?> change) {
    super.onChange(change);
    print(change);
  }

  @override
  void onTransition(Transition<SelectedMovieEvent, Movie?> transition) {
    super.onTransition(transition);
    print(transition);
  }
}