import 'package:bloc/bloc.dart';
import 'package:move_app/logic/cubits/movie_detail_state.dart';
import 'package:move_app/data/repositories/movie_repository.dart';
import 'package:move_app/logic/buisness_logic.dart';

class MovieDetailCubit extends Cubit<MovieDetailState> {

  final MovieRepository _movieRepository;

  MovieDetailCubit(this._movieRepository) : super(MovieDetailState());

  Future<void>fetchMovieDetail(String? id) async {
  
    if (id == null || id.isEmpty) return;

    emit(state.copyWith(status: MovieDetailStatus.loading));

    try {
      final movieDetail = await _movieRepository.fetchMovie(id);
      emit(
        state.copyWith(
          status: MovieDetailStatus.success,
          movieDetail: movieDetail
        )
      );
     } on Exception {
      emit(state.copyWith(status: MovieDetailStatus.failure));
    }
  }
}

