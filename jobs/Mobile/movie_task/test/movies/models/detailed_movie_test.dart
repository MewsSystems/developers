import 'package:flutter_test/flutter_test.dart';
import 'package:movie_task/movies/models/detailed_movie.dart';

void main() {
  group('Movie', () {
    test('only compares id, title and body', () {
      expect(
        const DetailedMovie(
          id: 1,
          title: 'post title',
          body: 'post body',
          image: 'abc',
        ),
        const DetailedMovie(
          id: 1,
          title: 'post title',
          body: 'post body',
          image: 'dfg',
        ),
      );
    });
  });
}
