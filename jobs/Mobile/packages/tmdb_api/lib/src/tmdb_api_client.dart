import 'dart:async';
import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:tmdb_api/src/models/models.dart';
import 'package:tmdb_api/auth/env.dart'
    as env; //ignored in git. Create /lib/auth/env.dart with your own TMDb api key.

class MovieNotFoundFailure implements Exception {}

class MovieRequestFailure implements Exception {}

class MovieSearchFailure implements Exception {}

class CreditsFailure implements Exception {}

class InvalidApiKeyFailure implements Exception {}

class TMDbApiClient {
  static const String _baseUrl = 'api.themoviedb.org';
  final http.Client _httpClient;

  TMDbApiClient({http.Client? httpClient})
      : _httpClient = httpClient ?? http.Client();

  Future<Movie> getMovie(int id) async {
    final Uri movieRequest = Uri.https(
        _baseUrl, '/3/movie/$id', <String, dynamic>{'api_key': env.apiKey});
    final http.Response movieResponse = await _httpClient.get(movieRequest);

    if (movieResponse.statusCode != 200) {
      switch (movieResponse.statusCode) {
        case 401:
          throw InvalidApiKeyFailure();
        case 404:
          throw MovieNotFoundFailure();
        default:
          throw MovieRequestFailure();
      }
    }

    final Map<String, dynamic> bodyJson =
        jsonDecode(movieResponse.body) as Map<String, dynamic>;

    return Movie.fromJson(bodyJson);
  }

  Future<SearchResult> searchMovie(String name, [int pageIndex = 1]) async {
    final Uri searchRequest = Uri.https(
      _baseUrl,
      '3/search/movie',
      <String, dynamic>{
        'api_key': env.apiKey,
        'query': name,
        'page': pageIndex.toString(),
      },
    );
    final http.Response searchResponse = await _httpClient.get(searchRequest);

    if (searchResponse.statusCode != 200) {
      switch (searchResponse.statusCode) {
        case 401:
          throw InvalidApiKeyFailure();
        case 404:
          throw MovieNotFoundFailure();
        default:
          throw MovieSearchFailure();
      }
    }

    return SearchResult.fromJson(jsonDecode(searchResponse.body));
  }

  Future<Credits> getMovieCredits(int id) async {
    final Uri creditsRequest = Uri.https(_baseUrl, '/3/movie/$id/credits',
        <String, dynamic>{'api_key': env.apiKey});
    final http.Response creditsResponse = await _httpClient.get(creditsRequest);

    if (creditsResponse.statusCode != 200) {
      switch (creditsResponse.statusCode) {
        case 401:
          throw InvalidApiKeyFailure();
        case 404:
          throw MovieNotFoundFailure();
        default:
          throw CreditsFailure();
      }
    }

    return Credits.fromJson(jsonDecode(creditsResponse.body));
  }
}
