import 'package:flutter_test/flutter_test.dart';
import 'package:movies/src/blocs/genres_cubit.dart';

void main() {
  group('GenresCubitTest', () {
    late GenresCubit genresCubit;

    setUp(() {
      genresCubit = GenresCubit();
    });

    test('initial state is empty', () {
      expect(genresCubit.state, <int, String>{});
    });

    test('fetch() requests and emits the genres data', () async {
      await genresCubit.fetch();
      expect(genresCubit.state, isNotEmpty);
    });
    
    tearDown(() {
      genresCubit.close();
    });
  });
}
