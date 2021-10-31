import 'dart:convert';

import 'package:either_dart/either.dart';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import 'package:movie_search/model/api_error/api_error.dart';
import 'package:movie_search/model/movie_details/movie_details.dart';
import 'package:movie_search/model/movies_list_response/movies_list_response.dart';

class ApiManager {
  static const String baseUrl = 'https://api.themoviedb.org/3/';
  static const String searchUrl = 'search/movie';
  static const String detailsUrl = 'movie/';
  static const String paramApiKey = 'api_key';
  static const String paramQuery = 'query';
  static const String paramPage = 'page';
  static const String apiKey = '03b8572954325680265531140190fd2a';
  static const String paramAuth = '$paramApiKey=$apiKey';

  Future<Either<ApiError, MoviesListResponse>> loadMovieList(String input, int page) async {
    var url = Uri.parse(baseUrl + searchUrl + '?$paramAuth&$paramQuery=$input&$paramPage=$page');
    var response = await http.get(url);

    try {
      if (response.statusCode != 200) {
        return Left(ApiError(code: response.statusCode, message: response.body));
      } else {
        return Right(await compute(_parseMoviesListResponse, response.body));
      }
    } on Exception catch (_) {
      debugPrint("List parsing error");
      return Left(ApiError(code: response.statusCode, message: "Parsing error"));
    }
  }

  Future<Either<ApiError, MovieDetails>> loadMovieDetails(int movieId) async {
    var url = Uri.parse(baseUrl + detailsUrl + '$movieId?$paramAuth');
    var response = await http.get(url);

    try {
      if (response.statusCode != 200) {
        return Left(ApiError(code: response.statusCode, message: response.body));
      } else {
        return Right(await compute(_parseMovieDetailsResponse, response.body));
      }
    } on Exception catch (_) {
      debugPrint("Details parsing error");
      return Left(ApiError(code: response.statusCode, message: "Parsing error"));
    }
  }

  static MoviesListResponse _parseMoviesListResponse(String body) {
    return MoviesListResponse.fromJson(jsonDecode(body));
  }

  static MovieDetails _parseMovieDetailsResponse(String body) {
    return MovieDetails.fromJson(jsonDecode(body));
  }
}
