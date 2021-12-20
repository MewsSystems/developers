import 'package:flutter_test/flutter_test.dart';
import 'package:move_app/data/models/movie_raw.dart';


void main() {
  group('MoviePreview serialization', () {
    test('fromJson', () {
      expect(
        MovieRaw.fromJson(const <String, dynamic>{
          'id': 1,
          'title': 'The World\'s Fastest Indian',
          'original_language': 'en',
          'release_date': '2005-11-11',
          'vote_average': 10.0,
          'poster_path': '/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg'
        }),
        equals(const MovieRaw(
          id: 1,
          title: 'The World\'s Fastest Indian',
          releaseDate: '2005-11-11',
          posterPath: '/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg')
        )
      );
    });
  });
}
