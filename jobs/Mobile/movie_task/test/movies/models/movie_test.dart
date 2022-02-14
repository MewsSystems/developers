import 'package:flutter_test/flutter_test.dart';
import 'package:movie_task/movies/models/movie.dart';

void main() {
  group('Movie', () {
    test('can compare movies', () {
      expect(
        const Movie(id: 1, title: 'post title', body: 'post body'),
        const Movie(id: 1, title: 'post title', body: 'post body'),
      );
    });
  });
}
