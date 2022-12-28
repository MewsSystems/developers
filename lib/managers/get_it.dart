import 'package:get_it/get_it.dart';

import '../scenes/home/repository/home_repository.dart';
import 'app_coordinator.dart';
import 'navigation_manager.dart';

GetIt getIt = GetIt.instance;

setupGetIt() {
  //Factories
  getIt.registerFactory<NavigationManager>(() => NavigationManager());
  getIt.registerFactory<HomeRepository>(() => HomeRepository());
  //Singletons
  getIt.registerSingleton<AppCoordinator>(AppCoordinator.shared);
}
