import 'dart:convert';

import 'package:movies/src/api/client.dart';
import 'package:movies/src/model/movie_details/movie_details.dart';

/// Superclass for MovieDetailsApi exceptions
abstract class MovieDetailsError implements Exception {}

/// Exception thrown when there is no movie matching the id
class MovieNotFoundError extends MovieDetailsError {}

/// Exception thrown when getMovieDetails fails
class MovieDetailsRequestError extends MovieDetailsError {}

class MovieDetailsApi {
  /// Gets the details of a specific movie by its [movieId]
  Future<MovieDetails> getMovieDetails(int movieId) async {
    final uri = getApiUri('/3/movie/$movieId');
    final response = await client.get(uri);
    if (response.statusCode != 200) {
      if (response.statusCode == 404) {
        throw MovieNotFoundError();
      }
      throw MovieDetailsRequestError();
    }

    final detailsJson = jsonDecode(response.body) as Map<String, dynamic>;
    
    return MovieDetails.fromJson(detailsJson);
  }
}
