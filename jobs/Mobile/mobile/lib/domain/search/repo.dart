import 'package:dartz/dartz.dart';
import 'package:mobile/domain/core/error.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';

/// {@template search_data}
/// Describes search api methods.
///
///A marker interface to be implemtented by all Repository library.
/// {@endtemplate}
abstract class SearchData {
  /// Get search.
  ///
  /// `query` Query to search.
  /// `page` Page to retrieve data.
  Future<Either<CinephileError, List<Movie>>> getSearch(String query,
      {String page});
}
