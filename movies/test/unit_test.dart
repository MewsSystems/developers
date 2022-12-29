import 'package:flutter_test/flutter_test.dart';
import 'package:movies/utils.dart';

void main() {
  group('utils', () {
    test('getApiImageUrl() builds a URL with the prefix, path and size', () {
      const path = '/path';
      const size = 500;
      final url = getApiImageUrl(path, size);

      expect(url.contains(apiImagePrefix), true);
      expect(url.contains(path), true);
      expect(url.contains(size.toString()), true);
    });

    test(
        'formatMinutes() formats 90 minutes to a String containing the 1 and 30 characters',
        () {
      final formattedString = formatMinutes(90);
      expect(formattedString.contains(1.toString()), true);
      expect(formattedString.contains(30.toString()), true);
    });
  });
}
