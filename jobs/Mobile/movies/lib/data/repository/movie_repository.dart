import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:movies/config/consts.dart';
import 'package:movies/core/errors/exceptions.dart';
import 'package:movies/models/detailed_movie_model.dart';
import 'package:movies/models/movie_search_response_model.dart';

abstract class MovieRepository {
  Future<MovieSearchResponse> getMovies(int page, String query);
  Future<DetailedMovie> getMovieById(int id);
}

class RemoteMovieRepository implements MovieRepository {
  RemoteMovieRepository(this.client);

  final http.Client client;

  @override
  Future<MovieSearchResponse> getMovies(int page, String query) async {
    final queryParameters = {
      'api_key': Api.key,
      'language': Api.locale,
      'query': query,
      'page': '$page',
      'include_adult': '${Api.includeAdult}',
    };

    final uri = Uri.https(
      Api.baseUrl,
      Api.searchMoviesPath,
      queryParameters,
    );

    final response = await client.get(
      uri,
      headers: {'Content-Type': 'application/json'},
    );

    if (response.statusCode == 200) {
      return MovieSearchResponse.fromJson(
        json.decode(response.body) as Map<String, dynamic>,
      );
    } else {
      throw ServerException();
    }
  }

  @override
  Future<DetailedMovie> getMovieById(int id) async {
    final queryParameters = {
      'api_key': Api.key,
      'language': Api.locale,
    };

    final uri = Uri.https(
      Api.baseUrl,
      '${Api.detailedMoviesPath}/$id',
      queryParameters,
    );

    final response = await client.get(
      uri,
      headers: {'Content-Type': 'application/json'},
    );

    if (response.statusCode == 200) {
      return DetailedMovie.fromJson(
        json.decode(response.body) as Map<String, dynamic>,
      );
    } else {
      throw ServerException();
    }
  }
}
