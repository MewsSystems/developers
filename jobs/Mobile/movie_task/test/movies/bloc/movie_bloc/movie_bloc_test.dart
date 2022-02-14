import 'package:bloc_test/bloc_test.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:movie_task/movies/bloc/movie_bloc/movie_bloc.dart';
import 'package:movie_task/movies/models/models.dart';
import 'package:movie_task/movies/repository/movie_repository.dart';

class MockMovieRepository extends Mock implements MovieRepository {}

void main() {
  group('Movie Bloc', () {
    const mockMovie = DetailedMovie(id: 1, title: 'test', body: 'test');

    late MovieRepository movieRepository;

    setUpAll(() {
      registerFallbackValue(Uri());
    });

    setUp(() {
      movieRepository = MockMovieRepository();
    });

    test('initial state is MovieState()', () {
      expect(
        MovieBloc(movieRepository: movieRepository).state,
        const MovieState(),
      );
    });

    group('MovieFetched', () {
      blocTest<MovieBloc, MovieState>(
        'emits successful status when repository fetches initial movies',
        setUp: () {
          when(
            () => movieRepository.getDetailedMovie(
              id: any(named: 'id'),
            ),
          ).thenAnswer(
            (_) async =>
                const DetailedMovie(id: 1, title: 'test', body: 'test'),
          );
        },
        build: () => MovieBloc(movieRepository: movieRepository),
        act: (bloc) => bloc
            .add(MovieFetched(const Movie(id: 1, title: 'test', body: 'test'))),
        expect: () => const <MovieState>[
          MovieState(
            status: MovieStatus.success,
            movie: mockMovie,
          )
        ],
        verify: (_) {
          verify(
            () => movieRepository.getDetailedMovie(
              id: any(named: 'id'),
            ),
          ).called(1);
        },
      );
    });
  });
}
