import 'package:bloc_test/bloc_test.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:movies/src/blocs/movie_search_bloc.dart';
import 'package:movies/src/model/movie_search/movie_search_state.dart';
import 'package:movies/utils.dart';

void main() {
  group('MovieSearchBlocTest', () {
    late MovieSearchBloc movieSearchBloc;

    setUp(() {
      movieSearchBloc = MovieSearchBloc();
    });

    test('initial state is empty', () {
      expect(movieSearchBloc.state, MovieSearchState.emptyResult);
    });

    blocTest<MovieSearchBloc, MovieSearchState>(
      'emits non-empty list result when QueryChanged is added',
      build: () => movieSearchBloc,
      act: (bloc) => bloc.add(QueryChanged('a')),
      wait: const Duration(seconds: 2),
      verify: (bloc) => expect(
        bloc.state.when(result: (_, list, __) => [list], error: (_) => null),
        isNotEmpty,
      ),
    );

    blocTest<MovieSearchBloc, MovieSearchState>(
      'emits empty result when DeleteQuery is added and the state was a non-empty list result',
      build: () => movieSearchBloc,
      seed: () => const MovieSearchState.result('a', [testMovie], false),
      act: (bloc) => bloc.add(DeleteQuery()),
      wait: const Duration(seconds: 1),
      expect: () => [
        MovieSearchState.emptyResult,
      ],
    );
    
    tearDown(() {
      movieSearchBloc.close();
    });
  });
}
