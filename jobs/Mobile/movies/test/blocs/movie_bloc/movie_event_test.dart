// ignore_for_file: prefer_const_constructors

import 'package:flutter_test/flutter_test.dart';
import 'package:movies/blocs/movie_bloc/movie_bloc.dart';

void main() {
  group('test MovieEvent', () {
    test('should supports value equality', () {
      expect(
        GetMovieEvent(123),
        equals(GetMovieEvent(123)),
      );
    });

    test('props are correct', () {
      expect(GetMovieEvent(123).props, equals(<Object>[123]));
    });
  });
}
