import 'package:get_it/get_it.dart';
import 'package:injectable/injectable.dart';

import 'injection.config.dart';

/// Get instanse of [GetIt]
final GetIt getIt = GetIt.instance;

/// initializes the configurations of [GetIt].
///
/// `env` environment
@injectableInit
void configureInjection(String env) {
  $initGetIt(getIt, environment: env);
}
