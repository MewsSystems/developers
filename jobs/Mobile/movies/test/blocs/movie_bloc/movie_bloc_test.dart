import 'package:bloc_test/bloc_test.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:movies/blocs/movie_bloc/movie_bloc.dart';
import 'package:movies/core/errors/exceptions.dart';
import 'package:movies/models/detailed_movie_model.dart';
import 'package:movies/networking/repository/movie_repository.dart';

class MockMovieRepository extends Mock implements MovieRepository {}

void main() {
  late MockMovieRepository mockMovieRepository;

  setUp(() {
    mockMovieRepository = MockMovieRepository();
  });

  MovieBloc buildBloc() => MovieBloc(movieRepository: mockMovieRepository);

  test('works properly', () {
    expect(buildBloc, returnsNormally);
  });

  test('initial state should be InitialMovieState', () {
    expect(buildBloc().state, equals(InitialMovieState()));
  });

  const tDetailedMovie = DetailedMovie(
    id: 123,
    backdropPath: '/backdropPath',
    posterPath: '/posterPath',
    originalTitle: 'title',
    releaseDate: '2017-01-01',
    voteAverage: 5.5,
    voteCount: 123,
    budget: 100,
    revenue: 101,
    overview: 'Lorem ipsum',
    tagline: 'xxx',
  );

  void setUpMockGetMoviesExeption() =>
      when(() => mockMovieRepository.getMovieById(any()))
          .thenThrow(ServerException());

  void setUpMockGetMoviesSuccess() =>
      when(() => mockMovieRepository.getMovieById(any()))
          .thenAnswer((_) async => tDetailedMovie);

  group('test GetMovieEvent', () {
    blocTest<MovieBloc, MovieState>(
      'should emit [Loading, Error] when api thow an error',
      setUp: setUpMockGetMoviesExeption,
      build: buildBloc,
      act: (MovieBloc bloc) async => bloc.add(const GetMovieEvent(123)),
      expect: () => [
        LoadingMovieState(),
        const ErrorMovieState(message: 'Ooops, something went wrong'),
      ],
      verify: (_) => verify(
        () => mockMovieRepository.getMovieById(any()),
      ).called(1),
    );

    blocTest<MovieBloc, MovieState>(
      'should emit [Loading, Success] when data is gotten successfully',
      setUp: setUpMockGetMoviesSuccess,
      build: buildBloc,
      act: (MovieBloc bloc) async => bloc.add(const GetMovieEvent(123)),
      expect: () => [
        LoadingMovieState(),
        const SuccessMovieState(tDetailedMovie),
      ],
      verify: (_) => verify(
        () => mockMovieRepository.getMovieById(any()),
      ).called(1),
    );
  });
}
