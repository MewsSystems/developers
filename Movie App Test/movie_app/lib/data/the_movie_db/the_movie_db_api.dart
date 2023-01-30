import 'dart:io';

import 'package:dio/dio.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:movie_app/models/movie.dart';
import 'package:movie_app/models/movie_info.dart';
import 'package:movie_app/models/popular_movies.dart';

//Diferent sizes that we can have from the images url params
enum PosterSize {
  w92,
  w154,
  w185,
  w342,
  w500,
  w780,
  original,
}

Map<PosterSize, String> _posterSizes = {
  PosterSize.w92: "w92",
  PosterSize.w154: "w154",
  PosterSize.w185: "w185",
  PosterSize.w342: "w342",
  PosterSize.w500: "w500",
  PosterSize.w780: "w780",
  PosterSize.original: "original",
};

//Service that interact with TheMovieDB API
class TheMovieDbService {
  static String tmdbBaseImageUrl = "http://image.tmdb.org/t/p/";

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

  static Uri searchMovie(String query) {
    return Uri(
      scheme: 'https',
      host: 'api.themoviedb.org',
      path: '3/search/movie',
      queryParameters: {
        'api_key': dotenv.get('TMDB_KEY'),
        'include_adult': 'false',
        'query': query,
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

  Future<MovieInfo?> getMovieInfo(int movieId) async {
    final url = movieDetails(movieId).toString();

    final response = await Dio().get(url);

    if (response.statusCode != 200) {
      throw HttpException(
        response.data,
      );
    }
    final movieInfo = MovieInfo.fromJson(response.data);

    return movieInfo;
  }

  Future<List<Movie>?> getSearchMovies(String query) async {
    final url = searchMovie(query).toString();

    final response = await Dio().get(url);

    if (response.statusCode != 200) {
      throw HttpException(
        response.data,
      );
    }
    final result = PopularMovies.fromJson(response.data);

    return result.movies;
  }

  static String imageUrl(String path, PosterSize size) {
    return tmdbBaseImageUrl + _posterSizes[size]! + path;
  }
}
