import 'package:bloc/bloc.dart';
import 'package:move_app/logic/cubits/movies_state.dart';
import 'package:move_app/data/repositories/movie_repository.dart';


class PreviewCubit extends Cubit<MoviesState> {

  final MovieRepository _movieRepository;

  PreviewCubit(this._movieRepository) : super(const MoviesState());

  Future<void>fetchMovies(String? name) async {
    if (name == null) return;

    if (name.isEmpty) {
      emit(
        state.copyWith(
          status: MoviesStatus.initial
        )
      );
      return;
    }

    emit(state.copyWith(status: MoviesStatus.loading));

    try {
      final moviePreview = await _movieRepository.fetchMovieDetail(name);
      emit(
        state.copyWith(
          status: MoviesStatus.success,
          moviesPreview: moviePreview
        )
      );
     } on Exception {
      emit(state.copyWith(status: MoviesStatus.failure));
    }
  }
}

