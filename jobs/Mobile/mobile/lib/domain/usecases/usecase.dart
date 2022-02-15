import 'package:dartz/dartz.dart';
import 'package:mobile/domain/core/error.dart';
import 'package:mobile/domain/search/repo.dart';

///Interface for creating easy to use Api.
abstract class Usecase<Type, Params> {
  final SearchData repo;

  const Usecase(this.repo);

  Future<Either<CinephileError, Type>> call(Params params);
}
