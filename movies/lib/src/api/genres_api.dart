import 'dart:convert';

import 'package:movies/src/api/client.dart';
import 'package:movies/src/model/movie.dart';

/// Exception thrown when searchMovies fails.
class GenresRequestFailure implements Exception {}

/// Exception thrown when the searched movie is not found.
class GenresNotFoundFailure implements Exception {}

class GenresApi {
  Future<Map<int, String>> getGenres() async {
    final uri = getApiUri(
      '/3/genre/movie/list',
    );
    final response = await client.get(uri);
    if (response.statusCode != 200) {
      throw GenresRequestFailure();
    }

    final locationJson = jsonDecode(response.body) as Map;

    if (!locationJson.containsKey('genres')) throw GenresNotFoundFailure();
    final results = locationJson['genres'] as List<dynamic>;
    final genres = <int, String>{};
    for (final element in results) {
      final map = element as Map<String, dynamic>;
      genres[map['id'] as int] = map['name'] as String;
    }
    if (genres.isEmpty) throw GenresNotFoundFailure();

    return genres;
  }
}
