// ignore_for_file: prefer_const_constructors, prefer_const_literals_to_create_immutables

import 'package:flutter_test/flutter_test.dart';
import 'package:movies/blocs/search_bloc/search_bloc.dart';

void main() {
  group('SearchState', () {
    test('should supports InitialSearchState value comparison', () {
      expect(InitialSearchState(), InitialSearchState());
    });

    test('should supports LoadingSearchState value comparison', () {
      expect(LoadingSearchState(), LoadingSearchState());
    });

    test('should supports SuccessSearchState value comparison', () {
      expect(
        SuccessSearchState(1, [], 0, false),
        SuccessSearchState(1, [], 0, false),
      );
    });

    test('should supports ErrorSearchState value comparison', () {
      expect(
        ErrorSearchState(message: 'error'),
        ErrorSearchState(message: 'error'),
      );
    });
  });
}
