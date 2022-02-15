import 'dart:developer';

import 'package:dio/dio.dart';
import 'package:injectable/injectable.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';
import 'package:mobile/domain/core/error.dart';
import 'package:dartz/dartz.dart';
import 'package:mobile/domain/search/repo.dart';
import 'package:mobile/infrastructure/core/endpoints.dart';
import 'package:mobile/infrastructure/search_impl/entity/error_entity/error.dart';
import 'package:mobile/infrastructure/core/utils.dart';
import 'package:mobile/infrastructure/search_impl/entity/movie_entity/movie_entity.dart';

///{@template client_registration_implemtation}
/// Implementaion of [SearchData].
/// {@endtemplate}
@LazySingleton(as: SearchData)
class SearchImpl extends SearchData {
  final Client _client = Client();

  @override
  Future<Either<CinephileError, List<Movie>>> getSearch(String query,
      {String page = '1'}) async {
    try {
      var response = await _client.get(Endpoints.query.search(query, page));

      final lastPage = response.data['total_pages'] ?? 0;
      return Right((response.data['results'] as List)
          .map((e) => MovieEntity.fromJson(e, lastPage).toModel())
          .toList());
    } on DioError catch (e) {
      return Left(CinephileErrorEntity.fromCode(e.type).toModel());
    } catch (_) {
      return Left(CinephileError.empty());
    }
  }
}
