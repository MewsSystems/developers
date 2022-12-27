import 'package:flutter/material.dart';

import 'common/widgets/waiting_screen.dart';
import 'managers/app_coordinator.dart';
import 'managers/get_it.dart';
import 'managers/navigation_manager.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  setupGetIt();
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
        title: 'Movies',
        theme: ThemeData(
          //
          scaffoldBackgroundColor: Colors.black,
          backgroundColor: Colors.black,
          primarySwatch: Colors.red,
        ),
        home: FutureBuilder(
          future: getIt.get<AppCoordinator>().init(),
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.done) {
              return getIt.get<NavigationManager>().homePage;
            }
            return const WaitingScreen();
          },
        ));
  }
}
