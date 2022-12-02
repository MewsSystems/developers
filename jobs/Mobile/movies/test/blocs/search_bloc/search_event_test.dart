// ignore_for_file: prefer_const_constructors

import 'package:flutter_test/flutter_test.dart';
import 'package:movies/blocs/search_bloc/search_bloc.dart';

void main() {
  group('test SearchEvent', () {
    group('test FirstSearchEvent', () {
      test('should supports value equality', () {
        expect(
          FirstSearchEvent('123'),
          equals(FirstSearchEvent('123')),
        );
      });

      test('props are correct', () {
        expect(FirstSearchEvent('123').props, equals(<Object>['123']));
      });
    });

    group('test NextSearchEvent', () {
      test('should supports value equality', () {
        expect(
          NextSearchEvent(),
          equals(NextSearchEvent()),
        );
      });

      test('props are correct', () {
        expect(NextSearchEvent().props, equals(<Object>[]));
      });
    });
  });
}
