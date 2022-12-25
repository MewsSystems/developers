import 'dart:convert';

import 'package:movies/src/api/client.dart';
import 'package:movies/src/model/movie.dart';

/// Superclass for MovieSearchApi exceptions
abstract class MovieSearchError implements Exception {}

/// Exception thrown when there is no movies found for the query
class NoMoviesFoundError extends MovieSearchError {}

/// Exception thrown when no query is provided
class NoQueryError extends MovieSearchError {}

/// Exception thrown when searchMovies fails
class MovieRequestError extends MovieSearchError {}

class MovieSearchApi {
  Future<List<Movie>> searchMovies(String query, {int page = 1}) async {
    final uri = getApiUri(
      '/3/search/movie',
      parameters: {
        'query': query,
        'page': page.toString(),
      },
    );
    final response = await client.get(uri);
    if (response.statusCode != 200) {
      if(response.statusCode == 422) {
        throw NoQueryError();
      }
      throw MovieRequestError();
    }

    final locationJson = jsonDecode(response.body) as Map;

    if (!locationJson.containsKey('results')) throw NoMoviesFoundError();

    final results = locationJson['results'] as List;

    if (results.isEmpty) throw NoMoviesFoundError();

    return [
      for (var result in results) Movie.fromJson(result as Map<String, dynamic>)
    ];
  }
}
