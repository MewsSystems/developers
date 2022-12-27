import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:integration_test/integration_test.dart';
import 'package:movies/main.dart' as app;
import 'package:movies/src/components/movie_chip.dart';
import 'package:movies/src/pages/details/details_page.dart';
import 'package:movies/src/pages/search/clear_query_button.dart';
import 'package:movies/src/pages/search/result_list.dart';
import 'package:movies/src/pages/search/search_bar.dart';
import 'package:movies/src/pages/search/top_button.dart';

void main() {
  IntegrationTestWidgetsFlutterBinding.ensureInitialized();

  testWidgets('end-to-end testing', (tester) async {
    app.main();
    await tester.pumpAndSettle();

    /// Typing a query in the search field shows movies
    await tester.enterText(find.byType(TextField), 'a');
    await tester.pumpAndSettle(const Duration(seconds: 2));
    expect(find.byType(MovieChip), findsWidgets);

    /// Tapping a movie tile shows the movie details
    await tester.tap(find.byType(MovieChip).first);
    await tester.pumpAndSettle();
    // Test the page
    expect(find.byType(DetailsPage), findsOneWidget);
    // Test the Movie data
    expect(find.byType(MovieChip), findsWidgets);
    // Test the MovieDetails data
    expect(find.text('Synopsis'), findsOneWidget);

    /// Tapping the back button closes the details page
    await tester.tap(find.byIcon(Icons.arrow_back));
    await tester.pumpAndSettle();
    expect(find.byType(SearchBar), findsOneWidget);

    /// Tapping the clear button clears the query
    await tester.tap(find.byType(ClearQueryButton));
    await tester.pumpAndSettle(const Duration(seconds: 1));
    expect(find.byType(MovieChip), findsNothing);

    /// Scrolling down and tapping the [TopButton] scrolls back up
    await tester.enterText(find.byType(TextField), 'a');
    await tester.pumpAndSettle(const Duration(seconds: 2));
    resultsScrollController
        .jumpTo(resultsScrollController.position.maxScrollExtent);
    await tester.pumpAndSettle();
    expect(find.byType(TopButton), findsOneWidget);
    await tester.tap(find.byType(TopButton));
    await tester.pumpAndSettle();
    expect(resultsScrollController.position.pixels, 0);

    // TODO : test pagination
  });
}
