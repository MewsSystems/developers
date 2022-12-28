import 'dart:convert';

import 'package:dartz/dartz.dart';
import 'package:http/http.dart' as http;

import '../../../common/models/failure.dart';
import '../../../utils/constants.dart';
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
        BASE_URL,
        MOVIE_SEARCH_ENDPOINT,
        request.toJson().map(
              (key, value) => MapEntry(key, value.toString()),
            ),
      );
      print('step a');
      final response = await http.get(url).timeout(const Duration(seconds: 10),
          onTimeout: () => throw ConnectionFailure());
      print('step b');

      final json = jsonDecode(utf8.decode(response.bodyBytes));
      final result = SearchMoviesResponse.fromJson(json);
      _cache[request.searchText.toLowerCase()] = result;

      print('step c');
      return Right(result);
    } on ConnectionFailure catch (e) {
      print('step connection failure');
      return Left(e);
    } catch (e) {
      print('step e');
      if (e.toString().toLowerCase().contains('failed host lookup')) {
        print('step f');
        return Left(ConnectionFailure());
      }
      print('step g');
      return Left(Failure(e.toString()));
    }
  }
}
