import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:movies/config/consts.dart';
import 'package:movies/core/errors/exceptions.dart';
import 'package:movies/models/detailed_movie_model.dart';
import 'package:movies/models/movie_search_response_model.dart';
import 'package:movies/networking/client/client.dart';
import 'package:movies/networking/repository/movie_repository.dart';

import '../../fixtures/fixture_reader.dart';

class MockClient extends Mock implements APIClient {}

void main() {
  registerFallbackValue(Uri());
  late RemoteMovieRepository repository;
  late MockClient client;

  setUp(() {
    client = MockClient();
    repository = RemoteMovieRepository(client: client);
  });

  const movieId = 545954;

  final movieSearchResponse = MovieSearchResponse.fromJson(
    json.decode(fixture('movies.json')) as Map<String, dynamic>,
  );

  final detailedMovie = DetailedMovie.fromJson(
    json.decode(fixture('movie.json')) as Map<String, dynamic>,
  );

  void setUpMockClient200getMovies(String file) {
    when(
      () => client.getMovies(any(), any(), any(), any(), any()),
    ).thenAnswer((_) async => movieSearchResponse);
  }

  void setUpMockClient404getMovies() {
    when(() => client.getMovies(any(), any(), any(), any(), any())).thenAnswer(
      (_) => Future.error(
        Response(
          data: 'Something went wrong',
          statusCode: 404,
          requestOptions: RequestOptions(path: ''),
        ),
      ),
    );
  }

  void setUpMockClient200getMovie(String file) {
    when(
      () => client.getMovieById(any(), any(), any()),
    ).thenAnswer((_) async => detailedMovie);
  }

  void setUpMockClient404getMovie() {
    when(() => client.getMovieById(any(), any(), any())).thenAnswer(
      (_) => Future.error(
        Response(
          data: 'Something went wrong',
          statusCode: 404,
          requestOptions: RequestOptions(path: ''),
        ),
      ),
    );
  }

  group('getMovies', () {
    test(
      '''should perform a GET request on a URL with number
       being the endpoint and with application/json header''',
      () async {
        // arrange
        setUpMockClient200getMovies('movies.json');
        // act
        await repository.getMovies(1, 'Doom');
        // assert
        verify(
          () => client.getMovies(
            Api.key,
            Api.locale,
            'Doom',
            1,
            false,
          ),
        );
      },
    );

    test(
      'should return MovieSearchResponse when the response code is 200 (success)',
      () async {
        // arrange
        setUpMockClient200getMovies('movies.json');
        // act
        final result = await repository.getMovies(1, 'Doom');
        // assert
        expect(result, equals(movieSearchResponse));
      },
    );

    test(
      'should throw a ServerException when the response code is 404 or other',
      () async {
        // arrange
        setUpMockClient404getMovies();
        // act
        final call = repository.getMovies;
        // assert
        expect(
          () => call(1, 'Doom'),
          throwsA(const TypeMatcher<ServerException>()),
        );
      },
    );
  });

  group('getMovieById', () {
    test(
      '''should perform a GET request on a URL with number
       being the endpoint and with application/json header''',
      () async {
        // arrange
        setUpMockClient200getMovie('movie.json');
        // act
        await repository.getMovieById(movieId);
        // assert
        verify(
          () => client.getMovieById(
            movieId,
            Api.key,
            Api.locale,
          ),
        );
      },
    );

    test(
      'should return MovieSearchResponse when the response code is 200 (success)',
      () async {
        // arrange
        setUpMockClient200getMovie('movie.json');
        // act
        final result = await repository.getMovieById(movieId);
        // assert
        expect(result, equals(detailedMovie));
      },
    );

    test(
      'should throw a ServerException when the response code is 404 or other',
      () async {
        // arrange
        setUpMockClient404getMovie();
        // act
        final call = repository.getMovieById;
        // assert
        expect(
          () => call(1),
          throwsA(const TypeMatcher<ServerException>()),
        );
      },
    );
  });
}
