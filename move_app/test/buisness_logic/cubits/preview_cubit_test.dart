import 'package:flutter/material.dart';
import 'package:bloc_test/bloc_test.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:move_app/data/repositories/movie.dart';
import 'package:move_app/logic/buisness_logic.dart';
import 'package:move_app/data/repositories/movie_repository.dart';
import 'package:mocktail/mocktail.dart';

class MockMovieRepository extends Mock implements MovieRepository {}

const searchString = 'Titanic';
void main() {
  group('PreviewCubit', () {

    late MovieRepository movieRepository;
    
    setUp(() {
      movieRepository = MockMovieRepository();
    });

    test('initial state is correct', () {
      final previewCubit = PreviewCubit(movieRepository);
      expect(previewCubit.state, const MoviesState(
        status: MoviesStatus.initial, movies: [])
      );
    });
    group('fetchWeather', () {
    blocTest<PreviewCubit, MoviesState>(
      'emits nothing when search string is null',
      build: () => PreviewCubit(movieRepository),
      act: (cubit) => cubit.fetchMovies(null),
      expect: () => <MoviesState>[],
      );
    
      blocTest<PreviewCubit, MoviesState>(
      'emits nothing when search is empty',
      build: () => PreviewCubit(movieRepository),
      act: (cubit) => cubit.fetchMovies(""),
      expect: () => [const MoviesState(status: MoviesStatus.initial),],
      );
        
      blocTest<PreviewCubit, MoviesState>(
        'throw error',
        setUp: () {
          when(
            () => movieRepository.fetchMovieDetail(any()),
          ).thenThrow(Exception('oooooooppps'));
        },
        build: () => PreviewCubit(movieRepository),
        act: (cubit) => cubit.fetchMovies(searchString),
        expect: () => <MoviesState>[ 
          const MoviesState(status: MoviesStatus.loading),
          const MoviesState(status: MoviesStatus.failure)],
        );

        blocTest<PreviewCubit, MoviesState>(
        'loading, success',
        setUp: () {
          when(
            () => movieRepository.fetchMovieDetail(any()),
          ).thenAnswer( (_) async => [
             Movie(
              id: 1, 
              title: 'title',
              releaseDate: '1.1.1', 
              image: Image.asset('assets/images/no-image.png'))]
          );
        },
        build: () => PreviewCubit(movieRepository),
        act: (cubit) => cubit.fetchMovies(searchString),
        expect: () => <MoviesState>[ 
         const MoviesState(status: MoviesStatus.loading, movies: []),
         MoviesState(status: MoviesStatus.success, movies: [
            Movie(
              id: 1, 
              title: 'title',
              releaseDate: '1.1.1', 
              image: Image.asset('assets/images/no-image.png'))]),
         ],
        );


      });

      
      


  });
}