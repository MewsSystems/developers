import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:move_app/constants.dart';
import '../models/models_raw.dart';

/// Exception thrown when fetch movies fails.
class FetchRawMoviesRequestFailure implements Exception {}

/// Exception thrown when fecth movie by id fails.
class FetchRawMovieDetailRequestFailure implements Exception {}

/// Exception thrown when movie for provided id is not found
class MovieDetailNotFoundFailure implements Exception {}

class MovieAPI {

  final http.Client _httpClient;

   MovieAPI({http.Client? httpClient}):
    _httpClient = httpClient ?? http.Client();

  Future<List<MovieRaw>>fetchRawMovies(String query) async {

    final movesRequest = Uri.https(
      API.baseURl,
      API.searchMoviesURl,
      {'query': query,'api_key': API.key},
    );

    final response = await _httpClient.get(movesRequest);

    if (response.statusCode != 200) {
      throw FetchRawMoviesRequestFailure();
    }

    final json = jsonDecode(
      response.body,
    )["results"] as List;

    List<MovieRaw> moviesPreview = [];

    for (var item in json) {
      moviesPreview.add(MovieRaw.fromJson(item)); 
    }

    return moviesPreview;
  }

  Future<MovieDetailRaw>fetchRawMovieDetail(String moveId) async {
    
    final movesRequest = Uri.https(
      API.baseURl,
      API.fetchMovieURl + moveId,
      {'api_key': API.key},
    );

    final response = await _httpClient.get(movesRequest);

    if (response.statusCode != 200) {
      throw FetchRawMovieDetailRequestFailure();
    }

    final json = jsonDecode(
      response.body,
    );

    if (json.isEmpty) {
      throw MovieDetailNotFoundFailure();
    }

    return MovieDetailRaw.fromJson(json);
  }

}
