import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:movies/src/model/movie.dart';

/// Exception thrown when searchMovies fails.
class MovieRequestFailure implements Exception {}

/// Exception thrown when the searched movie is not found.
class MovieNotFoundFailure implements Exception {}

class MovieApiClient {
  final client = http.Client();
  final baseUrl = 'api.themoviedb.org';
  final apiKey = '03b8572954325680265531140190fd2a';

  Future<List<Movie>> searchMovies(String query) async {
    final uri = Uri.https(
      baseUrl,
      '/3/search/movie',
      {
        'api_key': apiKey,
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
    ];
  }
}
