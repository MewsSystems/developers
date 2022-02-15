import 'package:injectable/injectable.dart';
import 'package:mobile/domain/core/error.dart';
import 'package:dartz/dartz.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';
import 'package:mobile/domain/search/model/search_params/search_params.dart';
import 'package:mobile/domain/search/repo.dart';
import 'package:mobile/domain/usecases/usecase.dart';

///Provies easy to use[SearchData] api.
@LazySingleton()
class GetSearch extends Usecase<List<Movie>, SearchParams> {
  GetSearch(SearchData repo) : super(repo);

  @override
  Future<Either<CinephileError, List<Movie>>> call(SearchParams params) async {
    return await repo.getSearch(params.query, page: params.page);
  }
}
