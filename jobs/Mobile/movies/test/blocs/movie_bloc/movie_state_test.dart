// ignore_for_file: prefer_const_constructors, prefer_const_literals_to_create_immutables

import 'package:flutter_test/flutter_test.dart';
import 'package:movies/blocs/movie_bloc/movie_bloc.dart';
import 'package:movies/models/detailed_movie_model.dart';

void main() {
  group('MovieState', () {
    test('should supports InitialMovieState value comparison', () {
      expect(InitialMovieState(), InitialMovieState());
    });

    test('should supports LoadingMovieState value comparison', () {
      expect(LoadingMovieState(), LoadingMovieState());
    });

    test('should supports SuccessMovieState value comparison', () {
      expect(
        SuccessMovieState(
          DetailedMovie(
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
          ),
        ),
        SuccessMovieState(
          DetailedMovie(
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
          ),
        ),
      );
    });

    test('should supports ErrorMovieState value comparison', () {
      expect(
        ErrorMovieState(message: 'error'),
        ErrorMovieState(message: 'error'),
      );
    });
  });
}
