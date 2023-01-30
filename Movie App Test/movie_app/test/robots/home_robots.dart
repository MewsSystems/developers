import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';

class HomeRobot {
  final WidgetTester tester;
  HomeRobot(this.tester);

  Future<void> clickOnAMovie() async {
    final movieCard = find.byKey(const Key('movieCard'));

    await tester.ensureVisible(movieCard);
    await tester.tap(movieCard);

    await tester.pumpAndSettle();
  }

  Future<void> scrollHomePage({bool scrollUp = false}) async {
    final scrollViewFinder = find.byKey(const Key('movieGridScrollView'));

    if (scrollUp) {
      await tester.fling(scrollViewFinder, const Offset(0, 500), 10000);
      await tester.pumpAndSettle();
    } else {
      await tester.fling(scrollViewFinder, const Offset(0, -500), 10000);
      await tester.pumpAndSettle();
    }
  }
}
