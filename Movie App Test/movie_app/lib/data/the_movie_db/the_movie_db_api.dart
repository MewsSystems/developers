import 'dart:convert';
import 'dart:io';

import 'package:dio/dio.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:http/http.dart' as http;
import 'package:movie_app/models/movie.dart';
import 'package:movie_app/models/popular_movies.dart';

//Service that interact with TheMovieDB API
class TheMovieDbService {
  static Uri popularMovies(int page) {
    return Uri(
      scheme: 'https',
      host: 'api.themoviedb.org',
      path: '3/movie/popular',
      queryParameters: {
        'api_key': dotenv.get('TMDB_KEY'),
        'include_adult': 'false',
        'page': '$page',
      },
    );
  }

  static Uri movieDetails(int movieId) {
    return Uri(
      scheme: 'https',
      host: 'api.themoviedb.org',
      path: '3/movie/$movieId',
      queryParameters: {
        'api_key': dotenv.get('TMDB_KEY'),
        'include_adult': 'false',
      },
    );
  }

  Future<List<Movie>?> getListOfPopularMovies({int page = 1}) async {
    final url = popularMovies(page).toString();

    final response = await Dio().get(url);

    if (response.statusCode != 200) {
      throw HttpException(
        response.data,
      );
    }
    final result = PopularMovies.fromJson(response.data);

    return result.movies;
  }
}
