import 'package:dio/dio.dart';
import 'package:logger/logger.dart';
import 'package:movies/config/consts.dart';
import 'package:movies/core/errors/exceptions.dart';
import 'package:movies/models/detailed_movie_model.dart';
import 'package:movies/models/movie_search_response_model.dart';
import 'package:movies/networking/client/client.dart';

abstract class MovieRepository {
  Future<MovieSearchResponse> getMovies(int page, String query);
  Future<DetailedMovie> getMovieById(int id);
}

class RemoteMovieRepository implements MovieRepository {
  RemoteMovieRepository({required APIClient client}) : _client = client;

  final APIClient _client;

  @override
  Future<MovieSearchResponse> getMovies(int page, String query) {
    final logger = Logger();

    return _client
        .getMovies(Api.key, Api.locale, query, page, false)
        .then((msr) => msr)
        .catchError((Object obj) {
      logger.e('obj : $obj');
      switch (obj.runtimeType) {
        case DioError:
          // Here's the sample to get the failed response error code and message
          final res = (obj as DioError).response;
          if (res != null) {
            logger.e('Got error : ${res.statusCode} -> ${res.statusMessage}');
          }
          throw ServerException();
        default:
          throw ServerException();
      }
    });
  }

  @override
  Future<DetailedMovie> getMovieById(int id) async {
    final logger = Logger();

    return _client
        .getMovieById(id, Api.key, Api.locale)
        .then((dm) => dm)
        .catchError((Object obj) {
      // non-200 error goes here.
      logger.e('obj : $obj');
      switch (obj.runtimeType) {
        case DioError:
          // Here's the sample to get the failed response error code and message
          final res = (obj as DioError).response;
          if (res != null) {
            logger.e('Got error : ${res.statusCode} -> ${res.statusMessage}');
          }
          throw ServerException();
        default:
          throw ServerException();
      }
    });
  }
}
