import 'package:bloc_test/bloc_test.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:movie_task/movies/bloc/search_bloc/search_bloc.dart';
import 'package:movie_task/movies/models/movie.dart';
import 'package:movie_task/movies/repository/movie_repository.dart';

class MockMovieRepository extends Mock implements MovieRepository {}

void main() {
  group('Movie Bloc', () {
    const mockMovies = [Movie(id: 1, title: 'post title', body: 'post body')];

    late MovieRepository movieRepository;

    setUpAll(() {
      registerFallbackValue(Uri());
    });

    setUp(() {
      movieRepository = MockMovieRepository();
    });

    test('initial state is SearchState()', () {
      expect(
        SearchBloc(movieRepository: movieRepository).state,
        const SearchState(),
      );
    });

    group('SearchFetched', () {
      blocTest<SearchBloc, SearchState>(
        'emits nothing when movies has reached maximum amount',
        build: () => SearchBloc(movieRepository: movieRepository),
        seed: () => const SearchState(hasReachedMax: true),
        act: (bloc) => bloc.add(SearchFetched()),
        expect: () => <SearchState>[],
      );

      blocTest<SearchBloc, SearchState>(
        'emits successful status when repository fetches initial posts',
        setUp: () {
          when(
            () => movieRepository.getMovies(
              query: any(named: 'query'),
              setPages: any(named: 'setPages'),
            ),
          ).thenAnswer((_) async => mockMovies);
        },
        build: () => SearchBloc(movieRepository: movieRepository),
        act: (bloc) => bloc.add(SearchFetched()),
        expect: () => const <SearchState>[
          SearchState(
            status: SearchStatus.success,
            movies: mockMovies,
            hasReachedMax: false,
          )
        ],
        verify: (_) {
          verify(
            () => movieRepository.getMovies(
              query: any(named: 'query'),
              setPages: any(named: 'setPages'),
            ),
          ).called(1);
        },
      );
    });
  });
}
