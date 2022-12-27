import 'dart:convert';

import 'package:dartz/dartz.dart';
import 'package:http/http.dart' as http;

import '../../../common/models/failure.dart';
import '../models/search_movies_request.dart';
import '../models/search_movies_response.dart';

class HomeRepository {
  final Map<String, SearchMoviesResponse> _cache = {};

  Future<Either<Failure, SearchMoviesResponse>> searchMovies(
      {required SearchMoviesRequest request}) async {
    if (_cache.containsKey(request.searchText.toLowerCase())) {
      return Right(_cache[request.searchText]!);
    }
    try {
      final url = Uri.https(
          'api.themoviedb.org',
          '/3/search/movie',
          request
              .toJson()
              .map((key, value) => MapEntry(key, value.toString())));

      final response = await http.get(url).timeout(const Duration(seconds: 10),
          onTimeout: () => throw ConnectionFailure());

      final json = jsonDecode(response.body);
      final result = SearchMoviesResponse.fromJson(json);
      _cache[request.searchText.toLowerCase()] = result;

      return Right(result);
    } on ConnectionFailure catch (e) {
      return Left(e);
    } catch (e) {
      return Left(Failure(e.toString()));
    }
  }
}
