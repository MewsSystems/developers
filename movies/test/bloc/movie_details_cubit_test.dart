import 'package:flutter_test/flutter_test.dart';
import 'package:movies/src/blocs/movie_details_cubit.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/model/movie_details/movie_details_state.dart';
import 'package:movies/utils.dart';

void main() {
  group('MovieDetailsCubitTest', () {
    late MovieDetailsCubit movieDetailsCubit;
    late SelectedMovieBloc selectedMovieBloc;

    setUp(() {
      selectedMovieBloc = SelectedMovieBloc();
      movieDetailsCubit = MovieDetailsCubit(selectedMovieBloc);
    });

    test('initial state is empty', () {
      expect(movieDetailsCubit.state, const MovieDetailsState.noSelection());
    });

    test('selecting a movie emits loading state and then details. Deselecting emits noSelection state', () async {
      selectedMovieBloc.add(SelectMovie(testMovie));
      await Future.delayed(Duration.zero);
      expect(movieDetailsCubit.state, const MovieDetailsState.loading());

      await Future.delayed(const Duration(seconds: 2));
      expect(movieDetailsCubit.state.whenOrNull(details: (d) => d), isNotNull);

      selectedMovieBloc.add(DeselectMovie());
      await Future.delayed(Duration.zero);
      expect(movieDetailsCubit.state, const MovieDetailsState.noSelection());
    });

    tearDown(() {
      movieDetailsCubit.close();
    });
  });
}
