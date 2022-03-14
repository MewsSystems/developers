import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:mews_imdb/movie_details/models/models.dart';

import 'package:movie_repository/movie_repository.dart' as rep;

part 'credits_state.dart';

class CreditsCubit extends Cubit<CreditsState> {
  CreditsCubit({
    required this.movieId,
    required this.movieRepository,
  }) : super(const CreditsState(status: CreditsStatus.init, membersList: []));

  final int movieId;
  final rep.MovieRepository movieRepository;

  void startLoading() async {
    emit(state.copyWith(status: CreditsStatus.loading));
    try {
      final List<rep.Member> result = await movieRepository.getCast(movieId);
      emit(
        state.copyWith(
          status: CreditsStatus.success,
          membersList: result
              .map((member) =>
                  Member(name: member.name, posterPath: member.posterPath))
              .toList(),
        ),
      );
    } catch (e) {
      emit(state.copyWith(status: CreditsStatus.failure));
    }
  }
}
