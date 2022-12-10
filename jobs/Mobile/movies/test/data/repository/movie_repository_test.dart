import 'package:dartz/dartz.dart';
import 'package:dio/dio.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:movies/config/consts.dart';
import 'package:movies/core/errors/exceptions.dart';
import 'package:movies/core/errors/network_exceptions.dart';
import 'package:movies/models/detailed_movie_model.dart';
import 'package:movies/models/movie_model.dart';
import 'package:movies/models/movie_search_response_model.dart';
import 'package:movies/networking/client/client.dart';
import 'package:movies/networking/repository/movie_repository.dart';

class MockClient extends Mock implements APIClient {}

void main() {
  registerFallbackValue(Uri());
  late RemoteMovieRepository repository;
  late MockClient client;

  setUp(() {
    client = MockClient();
    repository = RemoteMovieRepository(client: client);
  });

  final error = DioError(
    response: Response(
      data: 'Something went wrong',
      statusCode: 404,
      requestOptions: RequestOptions(path: ''),
    ),
    requestOptions: RequestOptions(path: ''),
    type: DioErrorType.cancel,
  );

  final networkException = NetworkExceptions.getDioException(error);

  group('getMovies', () {
    const movieSearchResponse = MovieSearchResponse(
      page: 1,
      results: [
        Movie(
          id: 123,
          backdropPath: '/9EpgaLS44zlXoTSAOhGiJmkIA4B.jpg',
          posterPath: '/eVjlW6aOjqEohH4Ph4PktyH4fMr.jpg',
          originalTitle: 'Doom',
          releaseDate: '2005-10-20',
          voteAverage: 5.1,
          voteCount: 1873,
        ),
      ],
      totalPages: 1,
      totalResults: 1,
    );

    test(
      'should return remote data when the call to remote data source is successful',
      () async {
        // arrange
        when(() => client.getMovies(any(), any(), any(), any(), any()))
            .thenAnswer((_) async => movieSearchResponse);
        // act
        final result = await repository.getMovies(1, 'Doom');
        // assert
        verify(() => client.getMovies(Api.key, Api.locale, 'Doom', 1, false));
        expect(
          result,
          equals(
            const Right<NetworkFailure, MovieSearchResponse>(
              movieSearchResponse,
            ),
          ),
        );
      },
    );

    test(
      'should return server failure when the call to remote data source is unsuccessful',
      () async {
        // arrange
        when(() => client.getMovies(any(), any(), any(), any(), any()))
            .thenThrow(error);
        // act
        final result = await repository.getMovies(1, 'Doom');
        // assert
        verify(() => client.getMovies(Api.key, Api.locale, 'Doom', 1, false));
        expect(
          result,
          equals(
            Left<NetworkFailure, MovieSearchResponse>(
              NetworkFailure(networkException),
            ),
          ),
        );
      },
    );
  });

  group('getMovies', () {
    const movieId = 123;

    const deDetailedMovie = DetailedMovie(
      id: movieId,
      backdropPath: '/9EpgaLS44zlXoTSAOhGiJmkIA4B.jpg',
      posterPath: '/eVjlW6aOjqEohH4Ph4PktyH4fMr.jpg',
      originalTitle: 'Doom',
      releaseDate: '2005-10-20',
      voteAverage: 5.1,
      voteCount: 1873,
      budget: 100000,
      overview: 'xxx',
      revenue: 200000,
      tagline: 'xxx',
    );

    test(
      'should return remote data when the call to remote data source is successful',
      () async {
        when(() => client.getMovieById(any(), any(), any()))
            .thenAnswer((_) async => deDetailedMovie);
        // act
        final result = await repository.getMovieById(movieId);
        // assert
        verify(() => client.getMovieById(Api.key, Api.locale, movieId));
        expect(
          result,
          equals(const Right<NetworkFailure, DetailedMovie>(deDetailedMovie)),
        );
      },
    );

    test(
      'should return server failure when the call to remote data source is unsuccessful',
      () async {
        // arrange
        when(() => client.getMovieById(any(), any(), any())).thenThrow(error);
        // act
        final result = await repository.getMovieById(movieId);
        // assert
        verify(() => client.getMovieById(Api.key, Api.locale, movieId));
        expect(
          result,
          equals(
            Left<NetworkFailure, DetailedMovie>(
              NetworkFailure(networkException),
            ),
          ),
        );
      },
    );
  });
}
