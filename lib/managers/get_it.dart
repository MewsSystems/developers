import 'package:get_it/get_it.dart';

import 'app_coordinator.dart';
import 'navigation_manager.dart';

GetIt getIt = GetIt.instance;

setupGetIt() {
  //Factories
  getIt.registerFactory<NavigationManager>(() => NavigationManager());
  //Singletons
  getIt.registerSingleton<AppCoordinator>(AppCoordinator.shared);
}
