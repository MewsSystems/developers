import 'package:bloc_test/bloc_test.dart';
import 'package:dartz/dartz.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:movies/blocs/movie_bloc/movie_bloc.dart';
import 'package:movies/core/errors/exceptions.dart';
import 'package:movies/core/errors/network_exceptions.dart';
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

  const movieId = 123;

  const detailedMovie = DetailedMovie(
    id: movieId,
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

  group('test GetMovieEvent', () {
    blocTest<MovieBloc, MovieState>(
      'should get data from the concrete use case',
      setUp: () {
        when(() => mockMovieRepository.getMovieById(any()))
            .thenAnswer((_) async => const Right(detailedMovie));
      },
      build: buildBloc,
      act: (MovieBloc bloc) async => bloc.add(const GetMovieEvent(movieId)),
      verify: (_) => verify(
        () => mockMovieRepository.getMovieById(any()),
      ).called(1),
    );

    blocTest<MovieBloc, MovieState>(
      'should emit [Loading, Success] when data is gotten successfully',
      setUp: () {
        when(() => mockMovieRepository.getMovieById(any()))
            .thenAnswer((_) async => const Right(detailedMovie));
      },
      build: buildBloc,
      act: (MovieBloc bloc) async => bloc.add(const GetMovieEvent(movieId)),
      expect: () => [
        LoadingMovieState(),
        const SuccessMovieState(detailedMovie),
      ],
      verify: (_) => verify(
        () => mockMovieRepository.getMovieById(any()),
      ).called(1),
    );

    blocTest<MovieBloc, MovieState>(
      'should emit [Loading, Error] when getting data fails',
      setUp: () {
        when(() => mockMovieRepository.getMovieById(any())).thenAnswer(
          (_) async =>
              Left(NetworkFailure(const NetworkExceptions.requestCancelled())),
        );
      },
      build: buildBloc,
      act: (MovieBloc bloc) async => bloc.add(const GetMovieEvent(movieId)),
      expect: () => [
        LoadingMovieState(),
        const ErrorMovieState(message: 'Request Cancelled'),
      ],
      verify: (_) => verify(
        () => mockMovieRepository.getMovieById(any()),
      ).called(1),
    );
  });
}
