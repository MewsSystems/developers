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
        const SuccessSearchState(1, '123', [], 0, false),
        const SuccessSearchState(1, '123', [], 0, false),
      );
    });

    test('should supports ErrorSearchState value comparison', () {
      expect(
        const ErrorSearchState(message: 'error'),
        const ErrorSearchState(message: 'error'),
      );
    });
  });
}
