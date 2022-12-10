import 'dart:async';

import 'package:dartz/dartz.dart';
import 'package:dio/dio.dart';
import 'package:movies/config/consts.dart';
import 'package:movies/core/errors/exceptions.dart';
import 'package:movies/core/errors/network_exceptions.dart';
import 'package:movies/models/detailed_movie_model.dart';
import 'package:movies/models/movie_search_response_model.dart';
import 'package:movies/networking/client/client.dart';

abstract class MovieRepository {
  Future<Either<Failure, MovieSearchResponse>> getMovies(
    int page,
    String query,
  );

  Future<Either<Failure, DetailedMovie>> getMovieById(int id);
}

class RemoteMovieRepository implements MovieRepository {
  RemoteMovieRepository({required APIClient client}) : _client = client;

  final APIClient _client;

  @override
  Future<Either<Failure, MovieSearchResponse>> getMovies(
    int page,
    String query,
  ) async {
    try {
      final movieSearchResponse = await _client
          .getMovies(Api.key, Api.locale, query, page, false)
          .then((msr) => msr);
      return Right(movieSearchResponse);
    } on DioError catch (e) {
      final exeption = NetworkExceptions.getDioException(e);
      return Left(NetworkFailure(exeption));
    }
  }

  @override
  Future<Either<Failure, DetailedMovie>> getMovieById(int id) async {
    try {
      final detailedMovie =
          await _client.getMovieById(Api.key, Api.locale, id).then((dm) => dm);
      return Right(detailedMovie);
    } on DioError catch (e) {
      final exeption = NetworkExceptions.getDioException(e);
      return Left(NetworkFailure(exeption));
    }
  }
}
