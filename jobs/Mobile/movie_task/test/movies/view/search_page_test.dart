import 'package:bloc_test/bloc_test.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:movie_task/movies/bloc/search_bloc/search_bloc.dart';
import 'package:movie_task/movies/models/models.dart';
import 'package:movie_task/movies/view/view.dart';
import 'package:optimus/optimus.dart';

class MockSearchBloc extends MockBloc<SearchEvent, SearchState>
    implements SearchBloc {}

extension on WidgetTester {
  Future<void> pumpSearchPage(SearchBloc searchBloc) => pumpWidget(
        MaterialApp(
          home: BlocProvider.value(
            value: searchBloc,
            child: const SearchPage(),
          ),
        ),
      );
}

void main() {
  late SearchBloc searchBloc;

  setUp(() {
    searchBloc = MockSearchBloc();
  });
  testWidgets('finds search page ...', (tester) async {
    when(() => searchBloc.state).thenReturn(const SearchState());
    await tester.pumpSearchPage(searchBloc);
    await tester.pumpAndSettle();
    expect(find.byType(MoviesList), findsOneWidget);
    expect(find.byType(OptimusInputField), findsOneWidget);
  });

  testWidgets('fetches more movies when user enters into the textfield',
      (tester) async {
    when(() => searchBloc.state).thenReturn(
      SearchState(
        status: SearchStatus.success,
        movies: List.generate(
          10,
          (i) => Movie(id: i, title: 'post title', body: 'post body'),
        ),
      ),
    );

    await tester.pumpSearchPage(searchBloc);

    final Finder inputField = find.byType(OptimusInputField);
    await tester.enterText(inputField, 't');
    verify(() => searchBloc.add(SearchFetched())).called(1);
  });
}
