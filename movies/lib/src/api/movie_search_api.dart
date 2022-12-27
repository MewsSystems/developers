import 'dart:convert';

import 'package:movies/src/api/client.dart';
import 'package:movies/src/model/movie/movie.dart';

/// Superclass for MovieSearchApi exceptions
abstract class MovieSearchError implements Exception {}

/// Exception thrown when there is no movies found for the query
class NoMoviesFoundError extends MovieSearchError {}

/// Exception thrown when no query is provided
class NoQueryError extends MovieSearchError {}

/// Exception thrown when searchMovies fails
class MovieRequestError extends MovieSearchError {}

/// Exception thrown when trying to load a non-existing page
class PageError extends MovieSearchError {}

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
      if (response.statusCode == 422) {
        throw NoQueryError();
      }

      throw MovieRequestError();
    }

    final responseJson = jsonDecode(response.body) as Map;

    if (!responseJson.containsKey('results')) throw NoMoviesFoundError();

    final results = responseJson['results'] as List;

    if (results.isEmpty) {
      if (page > 1) {
        throw PageError();
      }
      throw NoMoviesFoundError();
    }

    return [
      for (var result in results) Movie.fromJson(result as Map<String, dynamic>)
    ];
  }
}
