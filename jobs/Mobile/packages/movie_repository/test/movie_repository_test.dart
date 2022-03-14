import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';

import 'package:movie_repository/movie_repository.dart';
import 'package:tmdb_api/tmdb_api.dart' as tmdb_api;

class MockTMDbApiClient extends Mock implements tmdb_api.TMDbApiClient {}

class MockMoviePreview extends Mock implements tmdb_api.MoviePreview {}

class MockMovie extends Mock implements tmdb_api.Movie {}

class MockSearchResult extends Mock implements tmdb_api.SearchResult {}

void main() {
  group(
    'Movie Repository',
    (() {
      late tmdb_api.TMDbApiClient tmdbApiClient;
      late MovieRepository movieRepository;

      setUp(() {
        tmdbApiClient = MockTMDbApiClient();
        movieRepository = MovieRepository(
          apiClient: tmdbApiClient,
        );
      });

      group('Constructor', () {
        test('Can be created without injecting api client', () {
          expect(MovieRepository(), isNotNull);
        });

        group('getMoviesDetails', () {
          const movieId = 550;

          test('getMovieDetails called on correct movie', () async {
            try {
              await movieRepository.getMovieDetails(movieId);
            } catch (_) {}
            verify(() => tmdbApiClient.getMovie(movieId)).called(1);
          });

          test('Throw exception when getMovieDetails fails', () async {
            final exception = Exception('error');
            when(() => tmdbApiClient.getMovie(any())).thenThrow(exception);
            expect(
              () async => await movieRepository.getMovieDetails(movieId),
              throwsA(exception),
            );
          });

          test('Return correct movie on success', () async {
            final movie = MockMovie();
            when(() => movie.adult).thenReturn(false);
            when(() => movie.posterPath).thenReturn('mock-pp');
            when(() => movie.backdropPath).thenReturn('mock-bp');
            when(() => movie.budget).thenReturn(666);
            when(() => movie.genres).thenReturn(
                <tmdb_api.Genre>[tmdb_api.Genre(id: 18, name: 'mock')]);
            when(() => movie.homepage).thenReturn('mock-homepage');
            when(() => movie.id).thenReturn(90);
            when(() => movie.imdbId).thenReturn('mock-imdb');
            when(() => movie.originalLanguage).thenReturn('us');
            when(() => movie.originalTitle).thenReturn('mock title');
            when(() => movie.overview).thenReturn('mock overview');
            when(() => movie.popularity).thenReturn(7.0);
            when(() => movie.productionCompanies).thenReturn(<tmdb_api.Company>[
              tmdb_api.Company(name: 'mock', id: 777, originCountry: 'cze')
            ]);
            when(() => movie.productionCountries).thenReturn(<tmdb_api.Country>[
              tmdb_api.Country(iso: 'mock-iso', name: 'mock-country-name')
            ]);
            when(() => movie.releaseDate).thenReturn(DateTime(2017, 9, 7));
            when(() => movie.revenue).thenReturn(66000);
            when(() => movie.runtime).thenReturn(120);
            when(() => movie.spokenLanguages).thenReturn(<tmdb_api.Language>[
              tmdb_api.Language(iso: 'mock-lang-iso', name: 'mock-lang-name')
            ]);
            when(() => movie.status).thenReturn('released');
            when(() => movie.tagline).thenReturn('mock-tagline');
            when(() => movie.title).thenReturn('mock-title');
            when(() => movie.video).thenReturn(false);
            when(() => movie.voteAverage).thenReturn(3.0);
            when(() => movie.voteCount).thenReturn(16);
            when(() => tmdbApiClient.getMovie(any()))
                .thenAnswer((_) async => movie);

            final MovieDetails actual =
                await movieRepository.getMovieDetails(movieId);
            expect(
                actual,
                MovieDetails(
                  adult: false,
                  budget: 666,
                  genres: ['mock'],
                  id: 90,
                  title: 'mock-title',
                  posterPath: 'mock-pp',
                  releaseDate: DateTime(2017, 9, 7),
                  voteAverage: 3.0,
                  voteCount: 16,
                  backdrop: 'mock-bp',
                  overview: 'mock overview',
                  runtime: 120,
                ));
          });
        });
      });
    }),
  );
}
