import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:movie_task/movies/models/models.dart';

class MovieRepository {
  MovieRepository({required this.httpClient});

  final String baseUrl = 'api.themoviedb.org';
  final String apiKey = '03b8572954325680265531140190fd2a';

  final http.Client httpClient;

  Future<List<Movie>> getMovies({
    int page = 1,
    required String query,
    required Function(int) setPages,
  }) async {
    final response = await httpClient.get(
      Uri.https(
        baseUrl,
        '/4/search/movie',
        <String, String>{
          'api_key': apiKey,
          'query': query,
          'page': page.toString(),
          'include_adult': false.toString(),
        },
      ),
    );
    if (response.statusCode == 200) {
      final Map<dynamic, dynamic> body = json.decode(response.body) as Map;
      final int totalPages = body['total_pages'] as int;
      setPages(totalPages);
      final List<dynamic> movieList = body['results'] as List;

      return movieList.map((dynamic json) {
        final Map<String, dynamic> jsonMap = json as Map<String, dynamic>;

        return Movie.fromJson(jsonMap);
      }).toList();
    }
    throw Exception('error fetching movies');
  }

  Future<DetailedMovie> getDetailedMovie({required int id}) async {
    final response = await httpClient.get(
      Uri.https(
        baseUrl,
        '/3/movie/$id',
        <String, String>{
          'api_key': apiKey,
        },
      ),
    );
    if (response.statusCode == 200) {
      final Map<String, dynamic> body =
          json.decode(response.body) as Map<String, dynamic>;

      return DetailedMovie.fromJson(body);
    }
    throw Exception('error fetching movie');
  }
}
