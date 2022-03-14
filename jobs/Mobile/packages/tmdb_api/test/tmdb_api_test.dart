import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';

import 'package:tmdb_api/auth/env.dart' as env;

import 'package:tmdb_api/tmdb_api.dart';
import 'package:http/http.dart' as http;

class MockHttpClient extends Mock implements http.Client {}

class MockResponse extends Mock implements http.Response {}

class FakeUri extends Fake implements Uri {}

void main() {
  group('TMDbApiClient', () {
    late http.Client httpClient;
    late TMDbApiClient tmdbClient;

    setUpAll(() {
      registerFallbackValue(FakeUri());
    });

    setUp(() {
      httpClient = MockHttpClient();
      tmdbClient = TMDbApiClient(httpClient: httpClient);
    });

    group('Constructor', () {
      test('httpClient not required', (() {
        expect(TMDbApiClient(), isNotNull);
      }));

      group(
        'getMovie',
        () {
          const mockId = 550;
          const apiKey = env.apiKey;

          test('Makes correct https request', () async {
            final response = MockResponse();
            when(() => response.statusCode).thenReturn(200);
            when(() => response.body).thenReturn('[]');
            when(() => httpClient.get(any())).thenAnswer((_) async => response);

            try {
              await tmdbClient.getMovie(mockId);
            } catch (_) {
              verify(() => httpClient.get(Uri.https(
                  'api.themoviedb.org',
                  '/3/movie/$mockId',
                  <String, String>{'api_key': apiKey}))).called(1);
            }
          });

          test('Throws MovieRequestFailure on non 200 response', () async {
            final response = MockResponse();
            when(() => response.statusCode).thenReturn(400);
            when(() => httpClient.get((any())))
                .thenAnswer((_) async => response);

            expect(() async => await tmdbClient.getMovie(mockId),
                throwsA(isA<MovieRequestFailure>()));
          });

          test('Throws InvalidApiKeyFailure on 401 response', () async {
            final response = MockResponse();
            when(() => response.statusCode).thenReturn(401);
            when(() => response.body).thenReturn('[]');
            when(() => httpClient.get(any())).thenAnswer((_) async => response);
            await expectLater(
              tmdbClient.getMovie(mockId),
              throwsA(isA<InvalidApiKeyFailure>()),
            );
          });

          test('Throws MovieNotFoundFailure on 404 response', () async {
            final response = MockResponse();
            when(() => response.statusCode).thenReturn(404);
            when(() => response.body).thenReturn('[]');
            when(() => httpClient.get(any())).thenAnswer((_) async => response);
            await expectLater(
              tmdbClient.getMovie(mockId),
              throwsA(isA<MovieNotFoundFailure>()),
            );
          });

          test('Returns Movie on valid response', () async {
            final response = MockResponse();
            when((() => response.statusCode)).thenReturn(200);
            when((() => response.body)).thenReturn(
              '''{
                  "adult": false,
                  "backdrop_path": "/fCayJrkfRaCRCTh8GqN30f8oyQF.jpg",
                  "belongs_to_collection": null,
                  "budget": 63000000,
                  "genres": [
                    {
                      "id": 18,
                      "name": "Drama"
                    }
                  ],
                  "homepage": "",
                  "id": 550,
                  "imdb_id": "tt0137523",
                  "original_language": "en",
                  "original_title": "Fight Club",
                  "overview": "A ticking-time-bomb insomniac and a slippery soap salesman channel primal male aggression into a shocking new form of therapy. Their concept catches on, with underground \\"fight clubs\\" forming in every town, until an eccentric gets in the way and ignites an out-of-control spiral toward oblivion.",
                  "popularity": 0.5,
                  "poster_path": null,
                  "production_companies": [
                    {
                      "id": 508,
                      "logo_path": "/7PzJdsLGlR7oW4J0J5Xcd0pHGRg.png",
                      "name": "Regency Enterprises",
                      "origin_country": "US"
                    },
                    {
                      "id": 711,
                      "logo_path": null,
                      "name": "Fox 2000 Pictures",
                      "origin_country": ""
                    },
                    {
                      "id": 20555,
                      "logo_path": null,
                      "name": "Taurus Film",
                      "origin_country": ""
                    }
                  ],
                  "production_countries": [
                    {
                      "iso_3166_1": "US",
                      "name": "United States of America"
                    }
                  ],
                  "release_date": "1999-10-12",
                  "revenue": 100853753,
                  "runtime": 139,
                  "spoken_languages": [
                    {
                      "iso_639_1": "en",
                      "name": "English"
                    }
                  ],
                  "status": "Released",
                  "tagline": "How much can you know about yourself if you've never been in a fight?",
                  "title": "Fight Club",
                  "video": false,
                  "vote_average": 7.8,
                  "vote_count": 3439
                }''',
            );
            when((() => httpClient.get(any())))
                .thenAnswer((invocation) async => response);
            final actual = await tmdbClient.getMovie(mockId);
            expect(
              actual,
              isA<Movie>()
                  .having(
                    (p0) => p0.adult,
                    'adult',
                    false,
                  )
                  .having(
                    (p0) => p0.backdropPath,
                    'backdropPath',
                    'https://image.tmdb.org/t/p/w500/fCayJrkfRaCRCTh8GqN30f8oyQF.jpg',
                  )
                  .having(
                    (p0) => p0.budget,
                    'budget',
                    63000000,
                  )
                  .having(
                    (p0) => p0.genres,
                    'genres list',
                    isA<List<Genre>>().having(
                      (p0) => p0[0],
                      'first genre',
                      isA<Genre>()
                          .having(
                            (p0) => p0.name,
                            'genre name',
                            'Drama',
                          )
                          .having(
                            (p0) => p0.id,
                            'genre id',
                            18,
                          ),
                    ),
                  )
                  .having(
                    (p0) => p0.homepage,
                    'homepage',
                    '',
                  )
                  .having(
                    (p0) => p0.id,
                    'id',
                    550,
                  )
                  .having(
                    (p0) => p0.imdbId,
                    'imdbId',
                    'tt0137523',
                  )
                  .having(
                    (p0) => p0.originalLanguage,
                    'originalLanguage',
                    'en',
                  )
                  .having(
                    (p0) => p0.originalTitle,
                    'originalTitle',
                    'Fight Club',
                  )
                  .having(
                    (p0) => p0.overview,
                    'overview',
                    'A ticking-time-bomb insomniac and a slippery soap salesman channel primal male aggression into a shocking new form of therapy. Their concept catches on, with underground "fight clubs" forming in every town, until an eccentric gets in the way and ignites an out-of-control spiral toward oblivion.',
                  )
                  .having(
                    (p0) => p0.popularity,
                    'popularity',
                    0.5,
                  )
                  .having(
                    (p0) => p0.posterPath,
                    'posterPath',
                    null,
                  )
                  .having(
                    (p0) => p0.productionCompanies,
                    'productionCompanies',
                    isA<List<Company>>()
                        .having(
                          (p0) => p0[0],
                          'first production company',
                          isA<Company>()
                              .having(
                                (p0) => p0.id,
                                'first company id',
                                508,
                              )
                              .having(
                                (p0) => p0.logoPath,
                                'company logo path',
                                '/7PzJdsLGlR7oW4J0J5Xcd0pHGRg.png',
                              )
                              .having(
                                (p0) => p0.name,
                                'company name',
                                'Regency Enterprises',
                              )
                              .having((p0) => p0.originCountry,
                                  'first company origin country', 'US'),
                        )
                        .having(
                          (p0) => p0[1],
                          'second production company',
                          isA<Company>()
                              .having(
                                (p0) => p0.id,
                                'second company id',
                                711,
                              )
                              .having(
                                (p0) => p0.logoPath,
                                'second company logo path',
                                isNull,
                              )
                              .having(
                                (p0) => p0.name,
                                'second company name',
                                'Fox 2000 Pictures',
                              )
                              .having((p0) => p0.originCountry,
                                  'second company origin country', ''),
                        )
                        .having(
                          (p0) => p0[2],
                          'third production company',
                          isA<Company>()
                              .having(
                                (p0) => p0.id,
                                'third company id',
                                20555,
                              )
                              .having(
                                (p0) => p0.logoPath,
                                'third company logo path',
                                isNull,
                              )
                              .having(
                                (p0) => p0.name,
                                'third company name',
                                'Taurus Film',
                              )
                              .having((p0) => p0.originCountry,
                                  'third company origin country', ''),
                        ),
                  )
                  .having(
                    (p0) => p0.productionCountries,
                    'productionCountries',
                    isA<List<Country>>(),
                  )
                  .having(
                    (p0) => p0.releaseDate,
                    'date time release date',
                    DateTime(1999, 10, 12),
                  )
                  .having(
                    (p0) => p0.revenue,
                    'revenue',
                    100853753,
                  )
                  .having(
                    (p0) => p0.runtime,
                    'runtime',
                    139,
                  )
                  .having(
                    (p0) => p0.spokenLanguages,
                    'spokenLanguages',
                    isA<List<Language>>(),
                  )
                  .having(
                    (p0) => p0.status,
                    'status',
                    'Released',
                  )
                  .having(
                    (p0) => p0.tagline,
                    'tagline',
                    'How much can you know about yourself if you\'ve never been in a fight?',
                  )
                  .having(
                    (p0) => p0.title,
                    'title',
                    'Fight Club',
                  )
                  .having(
                    (p0) => p0.video,
                    'video',
                    false,
                  )
                  .having(
                    (p0) => p0.voteAverage,
                    'voteAverage',
                    7.8,
                  )
                  .having(
                    (p0) => p0.voteCount,
                    'voteCount',
                    3439,
                  ),
            );
          });
        },
      );

      group('searchMovie', () {
        const String query = 'home';

        test('makes correct http request', () async {
          final response = MockResponse();
          when(() => response.statusCode).thenReturn(200);
          when(() => response.body).thenReturn('{}');
          when(() => httpClient.get(any())).thenAnswer((_) async => response);
          try {
            await tmdbClient.searchMovie(query);
          } catch (_) {}
          verify(
            () => httpClient.get(
              Uri.https(
                  'api.themoviedb.org', '3/search/movie', <String, String>{
                'api_key': env.apiKey,
                'query': query,
                'page': '1',
              }),
            ),
          ).called(1);
        });

        test('throws MovieSearchFailure on non-200 response', () async {
          final response = MockResponse();
          when(() => response.statusCode).thenReturn(400);
          when(() => httpClient.get(any())).thenAnswer((_) async => response);
          expect(
            () async => await tmdbClient.searchMovie(query),
            throwsA(isA<MovieSearchFailure>()),
          );
        });
        test('throws InvalidApiKeyFailure on bad api key', () async {
          final response = MockResponse();
          when(() => response.statusCode).thenReturn(401);
          when(() => httpClient.get(any())).thenAnswer((_) async => response);
          expect(
            () async => await tmdbClient.searchMovie(query),
            throwsA(isA<InvalidApiKeyFailure>()),
          );
        });
        test('throws MovieNotFoundFailure on not found', () async {
          final response = MockResponse();
          when(() => response.statusCode).thenReturn(404);
          when(() => httpClient.get(any())).thenAnswer((_) async => response);
          expect(
            () async => await tmdbClient.searchMovie(query),
            throwsA(isA<MovieNotFoundFailure>()),
          );
        });

        test('return valid SearchResult on call', () async {
          final response = MockResponse();
          when(() => response.statusCode).thenReturn(200);
          when(() => response.body).thenReturn(
            '''{
              "page": 1,
              "results": [
                {
                  "poster_path": "/cezWGskPY5x7GaglTTRN4Fugfb8.jpg",
                  "adult": false,
                  "overview": "When an unexpected enemy emerges and threatens global safety and security, Nick Fury, director of the international peacekeeping agency known as S.H.I.E.L.D., finds himself in need of a team to pull the world back from the brink of disaster. Spanning the globe, a daring recruitment effort begins!",
                  "release_date": "2012-04-25",
                  "genre_ids": [
                    878,
                    28,
                    12
                  ],
                  "id": 24428,
                  "original_title": "The Avengers",
                  "original_language": "en",
                  "title": "The Avengers",
                  "backdrop_path": "/hbn46fQaRmlpBuUrEiFqv0GDL6Y.jpg",
                  "popularity": 7.353212,
                  "vote_count": 8503,
                  "video": false,
                  "vote_average": 7.33
                },
                {
                  "poster_path": "/7cJGRajXMU2aYdTbElIl6FtzOl2.jpg",
                  "adult": false,
                  "overview": "British Ministry agent John Steed, under direction from \\"Mother\\", investigates a diabolical plot by arch-villain Sir August de Wynter to rule the world with his weather control machine. Steed investigates the beautiful Doctor Mrs. Emma Peel, the only suspect, but simultaneously falls for her and joins forces with her to combat Sir August.",
                  "release_date": "1998-08-13",
                  "genre_ids": [
                    53
                  ],
                  "id": 9320,
                  "original_title": "The Avengers",
                  "original_language": "en",
                  "title": "The Avengers",
                  "backdrop_path": "/8YW4rwWQgC2JRlBcpStMNUko13k.jpg",
                  "popularity": 2.270454,
                  "vote_count": 111,
                  "video": false,
                  "vote_average": 4.7
                }
              ],
              "total_results": 2,
              "total_pages": 1
            }
          ''',
          );
          when(() => httpClient.get(any())).thenAnswer((_) async => response);
          final actual = await tmdbClient.searchMovie(query);
          expect(
              actual,
              isA<SearchResult>()
                  .having(
                    (p0) => p0.page,
                    'SearchResult page',
                    1,
                  )
                  .having(
                    (p0) => p0.results,
                    'SearchResult results',
                    isA<List<MoviePreview>>()
                        .having(
                          (p0) => p0[0],
                          'First result',
                          isA<MoviePreview>()
                              .having(
                                (p0) => p0.posterPath,
                                'poster path',
                                'https://image.tmdb.org/t/p/w500/cezWGskPY5x7GaglTTRN4Fugfb8.jpg',
                              )
                              .having(
                                (p0) => p0.adult,
                                'adult',
                                false,
                              )
                              .having((p0) => p0.overview, 'overview',
                                  'When an unexpected enemy emerges and threatens global safety and security, Nick Fury, director of the international peacekeeping agency known as S.H.I.E.L.D., finds himself in need of a team to pull the world back from the brink of disaster. Spanning the globe, a daring recruitment effort begins!')
                              .having(
                                (p0) => p0.releaseDate,
                                'first result release date',
                                DateTime(2012, 04, 25),
                              )
                              .having(
                                (p0) => p0.genreIds,
                                'genres ids',
                                isA<List<int>>()
                                    .having(
                                      (p0) => p0[0],
                                      'first id',
                                      isA<int>().having(
                                        (p0) => p0,
                                        'id',
                                        878,
                                      ),
                                    )
                                    .having(
                                      (p0) => p0[1],
                                      'second id',
                                      isA<int>().having(
                                        (p0) => p0,
                                        'id',
                                        28,
                                      ),
                                    )
                                    .having(
                                      (p0) => p0[2],
                                      'third id',
                                      isA<int>().having(
                                        (p0) => p0,
                                        'id',
                                        12,
                                      ),
                                    ),
                              )
                              .having(
                                (p0) => p0.id,
                                'id',
                                24428,
                              )
                              .having(
                                (p0) => p0.originalTitle,
                                'originalTitle',
                                'The Avengers',
                              )
                              .having(
                                (p0) => p0.originalLanguage,
                                'originalLanguage',
                                'en',
                              )
                              .having(
                                (p0) => p0.title,
                                'title',
                                'The Avengers',
                              )
                              .having(
                                (p0) => p0.backdropPath,
                                'backdropPath',
                                'https://image.tmdb.org/t/p/w500/hbn46fQaRmlpBuUrEiFqv0GDL6Y.jpg',
                              )
                              .having(
                                (p0) => p0.popularity,
                                'popularity',
                                7.353212,
                              )
                              .having(
                                (p0) => p0.voteCount,
                                'voteCount',
                                8503,
                              )
                              .having(
                                (p0) => p0.video,
                                'video',
                                false,
                              )
                              .having(
                                (p0) => p0.voteAverage,
                                'voteAverage',
                                7.33,
                              ),
                        )
                        .having(
                          (p0) => p0[1],
                          'Second result',
                          isA<MoviePreview>()
                              .having(
                                (p0) => p0.posterPath,
                                'poster path',
                                'https://image.tmdb.org/t/p/w500/7cJGRajXMU2aYdTbElIl6FtzOl2.jpg',
                              )
                              .having(
                                (p0) => p0.adult,
                                'adult',
                                false,
                              )
                              .having(
                                (p0) => p0.overview,
                                'overview',
                                'British Ministry agent John Steed, under direction from "Mother", investigates a diabolical plot by arch-villain Sir August de Wynter to rule the world with his weather control machine. Steed investigates the beautiful Doctor Mrs. Emma Peel, the only suspect, but simultaneously falls for her and joins forces with her to combat Sir August.',
                              )
                              .having(
                                (p0) => p0.releaseDate,
                                'second release date',
                                DateTime(1998, 08, 13),
                              )
                              .having(
                                (p0) => p0.genreIds,
                                'genres ids',
                                isA<List<int>>().having(
                                  (p0) => p0[0],
                                  'first id',
                                  isA<int>().having(
                                    (p0) => p0,
                                    'id',
                                    53,
                                  ),
                                ),
                              )
                              .having(
                                (p0) => p0.id,
                                'id',
                                9320,
                              )
                              .having(
                                (p0) => p0.originalTitle,
                                'originalTitle',
                                'The Avengers',
                              )
                              .having(
                                (p0) => p0.originalLanguage,
                                'originalLanguage',
                                'en',
                              )
                              .having(
                                (p0) => p0.title,
                                'title',
                                'The Avengers',
                              )
                              .having(
                                (p0) => p0.backdropPath,
                                'backdropPath',
                                'https://image.tmdb.org/t/p/w500/8YW4rwWQgC2JRlBcpStMNUko13k.jpg',
                              )
                              .having(
                                (p0) => p0.popularity,
                                'popularity',
                                2.270454,
                              )
                              .having(
                                (p0) => p0.voteCount,
                                'voteCount',
                                111,
                              )
                              .having(
                                (p0) => p0.video,
                                'video',
                                false,
                              )
                              .having(
                                (p0) => p0.voteAverage,
                                'voteAverage',
                                4.7,
                              ),
                        ),
                  )
                  .having(
                    (p0) => p0.totalResults,
                    'totalResults',
                    2,
                  )
                  .having(
                    (p0) => p0.totalPages,
                    'totalPages',
                    1,
                  ));
        });
      });
    });
  });
}
