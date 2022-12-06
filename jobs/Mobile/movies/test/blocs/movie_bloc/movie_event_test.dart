import 'package:flutter_test/flutter_test.dart';
import 'package:movies/blocs/movie_bloc/movie_bloc.dart';

void main() {
  group('test MovieEvent', () {
    test('should supports value equality', () {
      expect(
        const GetMovieEvent(123),
        equals(const GetMovieEvent(123)),
      );
    });

    test('props are correct', () {
      expect(const GetMovieEvent(123).props, equals(<Object>[123]));
    });
  });
}
