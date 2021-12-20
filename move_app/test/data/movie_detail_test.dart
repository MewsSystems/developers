import 'package:flutter_test/flutter_test.dart';
import 'package:move_app/data/models/movie_detail_raw.dart';


void main() {
  group('Movie serialization', () {
    test('fromJson', () {
      expect(
        MovieDetailRaw.fromJson(const <String, dynamic>{
          'budget': 100000,
          'genres': [{"id": 18, "name": "drama"},
            {"id": 11, "name": "Romantic"}],
          'overview': 'bla bla bla',
          'poster_path': '/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg',
          'release_date': '1990',
          'title': 'Terminator'
        }),
        equals(const MovieDetailRaw(
          budget: 100000,
          genres: [GenreRaw(id: 18, name: 'drama'), GenreRaw(id: 11, name: 'Romantic')],
          overview: 'bla bla bla',
          posterPath: '/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg',
          releaseDate: '1990',
          title: 'Terminator'
          )
        )
      );
    });
  });
}