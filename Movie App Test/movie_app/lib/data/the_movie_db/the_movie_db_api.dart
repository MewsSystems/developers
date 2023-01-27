import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;
import 'package:movie_app/models/movie.dart';
import 'package:movie_app/models/popular_movies.dart';

//Service that interact with TheMovieDB API
class TheMovieDbService {
  final String apiKey = '03b8572954325680265531140190fd2a';
  final String apiUrl = 'https://api.themoviedb.org/3/';

  Future<List<Movie>?> getListOfPopularMovies() async {
    final response = await http.get(
      Uri.parse('${apiUrl}movie/popular?api_key=$apiKey&language=en-US&page=1'),
    );

    if (response.statusCode != 200) {
      throw HttpException(
        response.body,
      );
    }
    final data = json.decode(response.body);
    final result = PopularMovies.fromJson(data);

    return result.movies;
  }
}
