import 'package:bloc_test/bloc_test.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:movie_task/movies/bloc/search_bloc/search_bloc.dart';
import 'package:movie_task/movies/models/movie.dart';
import 'package:movie_task/movies/view/view.dart';
import 'package:movie_task/movies/widgets/widgets.dart';

class MockSearchBloc extends MockBloc<SearchEvent, SearchState>
    implements SearchBloc {}

extension on WidgetTester {
  Future<void> pumpPostsList(SearchBloc searchBloc) => pumpWidget(
        MaterialApp(
          home: BlocProvider.value(
            value: searchBloc,
            child: const MoviesList(),
          ),
        ),
      );
}

void main() {
  final mockMovies = List<Movie>.generate(
    5,
    (i) => Movie(id: i, title: 'movie title', body: 'movie body'),
  );

  late SearchBloc searchBloc;

  setUp(() {
    searchBloc = MockSearchBloc();
  });

  group('Movies List', () {
    testWidgets(
        'finds start typing text '
        'when post status is initial', (tester) async {
      when(() => searchBloc.state).thenReturn(const SearchState());
      await tester.pumpPostsList(searchBloc);
      expect(find.text('Start typing to search'), findsOneWidget);
    });

    testWidgets(
        'finds no movies text '
        'when search status is success but with 0 movies', (tester) async {
      when(() => searchBloc.state).thenReturn(
        const SearchState(
          status: SearchStatus.success,
          movies: [],
          hasReachedMax: true,
        ),
      );
      await tester.pumpPostsList(searchBloc);
      expect(find.text('no movies found'), findsOneWidget);
    });

    testWidgets(
        'finds 5 movies and a loader when movies max is not reached yet',
        (tester) async {
      when(() => searchBloc.state).thenReturn(
        SearchState(
          status: SearchStatus.success,
          movies: mockMovies,
        ),
      );
      await tester.pumpPostsList(searchBloc);
      expect(find.byType(MovieListItem), findsNWidgets(5));
      expect(find.byType(Loading), findsOneWidget);
    });

    testWidgets('does not find loader when movie max is reached',
        (tester) async {
      when(() => searchBloc.state).thenReturn(
        SearchState(
          status: SearchStatus.success,
          movies: mockMovies,
          hasReachedMax: true,
        ),
      );
      await tester.pumpPostsList(searchBloc);
      expect(find.byType(Loading), findsNothing);
    });

    testWidgets('fetches more movies when scrolling to the bottom',
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
      await tester.pumpPostsList(searchBloc);
      await tester.drag(find.byType(MoviesList), const Offset(0, -500));
      verify(() => searchBloc.add(SearchFetched())).called(1);
    });
  });
}
