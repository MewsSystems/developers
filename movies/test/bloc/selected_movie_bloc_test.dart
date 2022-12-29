import 'package:bloc_test/bloc_test.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/model/movie/movie.dart';
import 'package:movies/utils.dart';

void main() {
  group('SelectedMovieBlocTest', () {
    late SelectedMovieBloc selectedMovieBloc;

    setUp(() {
      selectedMovieBloc = SelectedMovieBloc();
    });

    test('initial state is empty', () {
      expect(selectedMovieBloc.state, null);
    });

    blocTest<SelectedMovieBloc, Movie?>(
      'emits movie when it is selected',
      build: () => selectedMovieBloc,
      act: (bloc) => bloc.add(SelectMovie(testMovie)),
      expect: () => [testMovie],
    );

    blocTest<SelectedMovieBloc, Movie?>(
      'emits movie when it is selected',
      build: () => selectedMovieBloc,
      seed: () => testMovie,
      act: (bloc) => bloc.add(DeselectMovie()),
      expect: () => [null],
    );

    tearDown(() {
      selectedMovieBloc.close();
    });
  });
}
