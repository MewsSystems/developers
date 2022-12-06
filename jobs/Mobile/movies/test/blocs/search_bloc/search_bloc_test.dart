import 'package:bloc_test/bloc_test.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:movies/blocs/search_bloc/search_bloc.dart';
import 'package:movies/core/errors/exceptions.dart';
import 'package:movies/data/repository/movie_repository.dart';
import 'package:movies/models/movie_model.dart';
import 'package:movies/models/movie_search_response_model.dart';

class MockMovieRepository extends Mock implements MovieRepository {}

void main() {
  late MockMovieRepository mockMovieRepository;

  setUp(() {
    mockMovieRepository = MockMovieRepository();
  });

  SearchBloc buildBloc() => SearchBloc(movieRepository: mockMovieRepository);

  test('works properly', () {
    expect(buildBloc, returnsNormally);
  });

  test('initial state should be InitialSearchState', () {
    expect(buildBloc().state, equals(InitialSearchState()));
  });

  const tQuery = '123';

  const tMovies = [
    Movie(
      id: 123,
      backdropPath: '/backdropPath',
      posterPath: '/posterPath',
      originalTitle: 'title',
      releaseDate: '2017-01-01',
      voteAverage: 5.5,
      voteCount: 123,
    )
  ];

  const tMovieSearchResponse = MovieSearchResponse(
    page: 1,
    results: tMovies,
    totalPages: 1,
    totalResults: 1,
  );

  void setUpMockGetMoviesExeption() =>
      when(() => mockMovieRepository.getMovies(any(), any()))
          .thenThrow(ServerException());

  void setUpMockGetMoviesSuccess() =>
      when(() => mockMovieRepository.getMovies(any(), any()))
          .thenAnswer((_) async => tMovieSearchResponse);

  group('test FirstSearchEvent', () {
    blocTest<SearchBloc, SearchState>(
      'should emit [Loading, Error] when api thow an error',
      setUp: setUpMockGetMoviesExeption,
      build: buildBloc,
      act: (SearchBloc bloc) async => bloc.add(const FirstSearchEvent('123')),
      expect: () => [
        LoadingSearchState(),
        const ErrorSearchState(message: 'Ooops, something went wrong'),
      ],
      verify: (_) => verify(
        () => mockMovieRepository.getMovies(any(), any()),
      ).called(1),
    );

    blocTest<SearchBloc, SearchState>(
      'should emit [Loading, Success] when data is gotten successfully',
      setUp: setUpMockGetMoviesSuccess,
      build: buildBloc,
      act: (SearchBloc bloc) async => bloc.add(const FirstSearchEvent('123')),
      expect: () => [
        LoadingSearchState(),
        const SuccessSearchState(1, tQuery, tMovies, 1, false),
      ],
      verify: (_) => verify(
        () => mockMovieRepository.getMovies(any(), any()),
      ).called(1),
    );
  });

  group('NextSearchEvent', () {
    blocTest<SearchBloc, SearchState>(
      'should emit [Error] when api thow an error',
      setUp: setUpMockGetMoviesExeption,
      build: buildBloc,
      seed: () => const SuccessSearchState(1, tQuery, tMovies, 1, false),
      act: (SearchBloc bloc) async => bloc.add(NextSearchEvent()),
      expect: () =>
          [const ErrorSearchState(message: 'Ooops, something went wrong')],
      verify: (_) => verify(
        () => mockMovieRepository.getMovies(any(), any()),
      ).called(1),
    );

    blocTest<SearchBloc, SearchState>(
      'should emit [Success] when data is gotten successfully',
      setUp: setUpMockGetMoviesSuccess,
      build: buildBloc,
      seed: () => const SuccessSearchState(1, tQuery, tMovies, 1, false),
      act: (SearchBloc bloc) async => bloc.add(NextSearchEvent()),
      expect: () => [const SuccessSearchState(2, tQuery, tMovies, 1, false)],
      verify: (_) => verify(
        () => mockMovieRepository.getMovies(any(), any()),
      ).called(1),
    );

    blocTest<SearchBloc, SearchState>(
      'should emit [Error] when current state is not Success',
      setUp: setUpMockGetMoviesSuccess,
      build: buildBloc,
      seed: InitialSearchState.new,
      act: (SearchBloc bloc) async => bloc.add(NextSearchEvent()),
      expect: () =>
          [const ErrorSearchState(message: 'Ooops, something went wrong')],
      verify: (_) => verifyNever(
        () => mockMovieRepository.getMovies(any(), any()),
      ),
    );
  });
}
