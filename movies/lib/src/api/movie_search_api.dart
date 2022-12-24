import 'dart:convert';

import 'package:movies/src/api/client.dart';
import 'package:movies/src/model/movie.dart';

/// Exception thrown when searchMovies fails.
class MovieRequestFailure implements Exception {}

/// Exception thrown when the searched movie is not found.
class MovieNotFoundFailure implements Exception {}

class MovieSearchApi {
  Future<List<Movie>> searchMovies(String query) async {
    final uri = getApiUri(
      '/3/search/movie',
      parameters: {
        'query': query,
      },
    );
    final response = await client.get(uri);
    if (response.statusCode != 200) {
      throw MovieRequestFailure();
    }

    final locationJson = jsonDecode(response.body) as Map;

    if (!locationJson.containsKey('results')) throw MovieNotFoundFailure();

    final results = locationJson['results'] as List;

    if (results.isEmpty) throw MovieNotFoundFailure();

    return [
      for (var result in results) Movie.fromJson(result as Map<String, dynamic>)
    ]..sort((a, b) => b.popularity.compareTo(a.popularity));
  }
}
