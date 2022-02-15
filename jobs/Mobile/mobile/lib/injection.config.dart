// GENERATED CODE - DO NOT MODIFY BY HAND

// **************************************************************************
// InjectableConfigGenerator
// **************************************************************************

import 'package:get_it/get_it.dart' as _i1;
import 'package:injectable/injectable.dart' as _i2;

import 'application/search/search_bloc.dart' as _i6;
import 'domain/search/repo.dart' as _i3;
import 'domain/usecases/search.dart' as _i5;
import 'infrastructure/search_impl/repo.dart'
    as _i4; // ignore_for_file: unnecessary_lambdas

// ignore_for_file: lines_longer_than_80_chars
/// initializes the registration of provided dependencies inside of [GetIt]
_i1.GetIt $initGetIt(_i1.GetIt get,
    {String? environment, _i2.EnvironmentFilter? environmentFilter}) {
  final gh = _i2.GetItHelper(get, environment, environmentFilter);
  gh.lazySingleton<_i3.SearchData>(() => _i4.SearchImpl());
  gh.lazySingleton<_i5.GetSearch>(() => _i5.GetSearch(get<_i3.SearchData>()));
  gh.factory<_i6.SearchBloc>(
      () => _i6.SearchBloc(getSearch: get<_i5.GetSearch>()));
  return get;
}
