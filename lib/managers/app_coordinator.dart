class AppCoordinator {
  static final AppCoordinator shared = AppCoordinator._();

  AppCoordinator._();

  Future<void> init() async {
    //Initializes all managers/providers here example UserManager, NotificationManager...
  }
}
