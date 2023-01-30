// This is a basic Flutter widget test.
//
// To perform an interaction with a widget in your test, use the WidgetTester
// utility in the flutter_test package. For example, you can send tap and scroll
// gestures. You can also use WidgetTester to find child widgets in the widget
// tree, read text, and verify that the values of widget properties are correct.

import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:integration_test/integration_test.dart';
import 'package:movie_app/main.dart' as app;

import 'robots/home_robots.dart';
import 'robots/movie_detail_robot.dart';

void main() {
  IntegrationTestWidgetsFlutterBinding.ensureInitialized();

  HomeRobot homeRobot;
  MovieDetailRobot movieDetailRobot;

  group('end-to-end test', () {
    testWidgets('whole app', (WidgetTester tester) async {
      app.main();

      await tester.pumpAndSettle();
      homeRobot = HomeRobot(tester);
      movieDetailRobot = MovieDetailRobot(tester);

      await homeRobot.scrollHomePage();
      await homeRobot.clickOnAMovie();

      await movieDetailRobot.scrollPage(scrollUp: true);
      await movieDetailRobot.scrollPage();
       await movieDetailRobot.goBack();

    });
  });
}
