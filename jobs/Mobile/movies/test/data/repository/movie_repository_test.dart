// ignore_for_file: prefer_const_constructors

import 'dart:convert';

import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:mocktail/mocktail.dart';
import 'package:movies/config/consts.dart';
import 'package:movies/core/errors/exceptions.dart';
import 'package:movies/data/repository/movie_repository.dart';
import 'package:movies/models/detailed_movie_model.dart';
import 'package:movies/models/movie_search_response_model.dart';

import '../../fixtures/fixture_reader.dart';

class MockHttpClient extends Mock implements http.Client {}

void main() {
  registerFallbackValue(Uri());
  late RemoteMovieRepository repository;
  late MockHttpClient mockHttpClient;

  setUp(() {
    mockHttpClient = MockHttpClient();
    repository = RemoteMovieRepository(client: mockHttpClient);
  });

  void setUpMockHttpClientSuccess200(String file) {
    when(() => mockHttpClient.get(any(), headers: any(named: 'headers')))
        .thenAnswer((_) async => http.Response(fixture(file), 200));
  }

  void setUpMockHttpClientFailure404() {
    when(() => mockHttpClient.get(any(), headers: any(named: 'headers')))
        .thenAnswer((_) async => http.Response('Something went wrong', 404));
  }

  final tMovieSearchResponse = MovieSearchResponse.fromJson(
    json.decode(fixture('movies.json')),
  );

  final tDetailedMovie = DetailedMovie.fromJson(
    json.decode(fixture('movie.json')),
  );

  group('getMovies', () {
    final queryParameters = {
      'api_key': Api.key,
      'language': Api.locale,
      'query': 'Doom',
      'page': '1',
      'include_adult': '${Api.includeAdult}',
    };

    final uri = Uri.https(
      Api.baseUrl,
      Api.searchMoviesPath,
      queryParameters,
    );

    test(
      '''should perform a GET request on a URL with number
       being the endpoint and with application/json header''',
      () async {
        // arrange
        setUpMockHttpClientSuccess200('movies.json');
        // act
        repository.getMovies(1, 'Doom');
        // assert
        verify(
          () => mockHttpClient.get(
            uri,
            headers: {'Content-Type': 'application/json'},
          ),
        );
      },
    );

    test(
      'should return MovieSearchResponse when the response code is 200 (success)',
      () async {
        // arrange
        setUpMockHttpClientSuccess200('movies.json');
        // act
        final result = await repository.getMovies(1, 'Doom');
        // assert
        expect(result, equals(tMovieSearchResponse));
      },
    );

    test(
      'should throw a ServerException when the response code is 404 or other',
      () async {
        // arrange
        setUpMockHttpClientFailure404();
        // act
        final call = repository.getMovies;
        // assert
        expect(
          () => call(1, 'Doom'),
          throwsA(TypeMatcher<ServerException>()),
        );
      },
    );
  });

  group('getMovieById', () {
    final queryParameters = {
      'api_key': Api.key,
      'language': Api.locale,
    };

    final uri = Uri.https(
      Api.baseUrl,
      '${Api.detailedMoviesPath}/1',
      queryParameters,
    );

    test(
      '''should perform a GET request on a URL with number
       being the endpoint and with application/json header''',
      () async {
        // arrange
        setUpMockHttpClientSuccess200('movie.json');
        // act
        repository.getMovieById(1);
        // assert
        verify(
          () => mockHttpClient.get(
            uri,
            headers: {'Content-Type': 'application/json'},
          ),
        );
      },
    );

    test(
      'should return MovieSearchResponse when the response code is 200 (success)',
      () async {
        // arrange
        setUpMockHttpClientSuccess200('movie.json');
        // act
        final result = await repository.getMovieById(1);
        // assert
        expect(result, equals(tDetailedMovie));
      },
    );

    test(
      'should throw a ServerException when the response code is 404 or other',
      () async {
        // arrange
        setUpMockHttpClientFailure404();
        // act
        final call = repository.getMovieById;
        // assert
        expect(() => call(1), throwsA(TypeMatcher<ServerException>()));
      },
    );
  });
}
