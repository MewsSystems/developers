import 'package:either_dart/either.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movie_search/api/api_manager.dart';
import 'package:movie_search/model/api_error/api_error.dart';
import 'package:movie_search/model/movie_details/movie_details.dart';

part 'details_cubit.freezed.dart';
part 'details_state.dart';

class DetailsCubit extends Cubit<DetailsState> {
  final ApiManager apiManager;

  DetailsCubit(this.apiManager) : super(const DetailsState.initial());

  void loadDetails(int movieId) async {
    try {
      emit(const DetailsState.loading());
      Either<ApiError, MovieDetails> response = await apiManager.loadMovieDetails(movieId);
      response.fold((ApiError error) => emit(DetailsState.error(error.message ?? '')), (MovieDetails response) {
        emit(DetailsState.loadedData(response));
      });
    } on Exception catch (e) {
      emit(const DetailsState.error('Data loading error'));
    }
  }
}
