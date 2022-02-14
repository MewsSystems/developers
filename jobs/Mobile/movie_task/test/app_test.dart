import 'package:flutter_test/flutter_test.dart';
import 'package:movie_task/app.dart';
import 'package:movie_task/movies/view/view.dart';

void main() {
  testWidgets('app ...', (tester) async {
    await tester.pumpWidget(App());
    await tester.pumpAndSettle();
    expect(find.byType(SearchPage), findsOneWidget);
  });
}
