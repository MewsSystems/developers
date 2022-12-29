import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:movies/src/components/movie_chip.dart';

void main() {
  group('MovieChip', () {
    testWidgets('should display the label', (WidgetTester tester) async {
      const testString = 'test';
      const widget = MaterialApp(
        home: Scaffold(
          body: MovieChip(label: testString),
        ),
      );

      // Build the MovieChip and trigger a frame.
      await tester.pumpWidget(widget);

      // Verify the MovieChip shows with the text
      expect(find.text(testString), findsOneWidget);
    });

    testWidgets('should display the icon', (WidgetTester tester) async {
      const testIcon = Icons.star;
      const widget = MaterialApp(
        home: Scaffold(
          body: MovieChip(
            label: '',
            icon: testIcon,
          ),
        ),
      );

      // Build the MovieChip and trigger a frame.
      await tester.pumpWidget(widget);

      // Verify the MovieChip shows with the icon
      expect(find.byIcon(testIcon), findsOneWidget);
    });
  });
}
