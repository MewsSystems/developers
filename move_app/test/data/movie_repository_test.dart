import 'package:flutter_test/flutter_test.dart';
import 'package:move_app/constants.dart';
import 'package:move_app/data/data_providers/move_api.dart';
import 'package:move_app/data/models/models_raw.dart';
import 'package:move_app/data/repositories/movie.dart';
import 'package:move_app/data/repositories/movie_detail.dart';
import 'package:move_app/data/repositories/movie_repository.dart';
import 'package:mocktail/mocktail.dart';
import 'package:flutter/material.dart';


class MockMovieAPI extends Mock implements MovieAPI {}

class MockMoviewPreview extends Mock implements MovieDetailRaw {}

class MockMovie extends Mock implements MovieDetailRaw {}

void main() {
  group('MovieRepository', () {
    late MovieRepository movieRepository;
    late MovieAPI moveAPI;

    setUp(() {
      moveAPI = MockMovieAPI();
      movieRepository = MovieRepository(moveAPI: moveAPI);
    });

      test('return movies preview on valid response', () async {
      when(() => moveAPI.fetchRawMovies(any()))
        .thenAnswer((_) async => const [MovieRaw(
          id: 1,
          title: 'The Worlds Fastest Indian',
          releaseDate:  '2005-11-11',
          posterPath: '/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg'
          )]);
        final actual = await movieRepository.fetchMovieDetail('');
      expect(
       actual,
       [Movie(
          id: 1,
          title: 'The Worlds Fastest Indian',
          releaseDate: '2005-11-11',
          image: Image.network(API.imageURL + '/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg'))]
        );
    });

      test('return movie detail on valid response', () async {
      when(() => moveAPI.fetchRawMovieDetail(any()))
        .thenAnswer((_) async => const MovieDetailRaw(
          budget: 1, 
          genres: [GenreRaw(id: 1, name: 'romantic')], 
          overview: 'bla bla bla ', 
          posterPath: '/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg', 
          title: 'robocop', 
          releaseDate: '1990'));
        
        final actual = await movieRepository.fetchMovie('');
      expect(
       actual,
         MovieDetail(
          budget: 1,
          genres: 'romantic  ', 
          overview: 'bla bla bla ', 
          title: 'robocop', 
          image: Image.network(API.imageURL + '/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg'))
        );
    });
  });
}
