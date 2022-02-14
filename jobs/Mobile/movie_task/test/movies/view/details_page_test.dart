import 'package:bloc_test/bloc_test.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:movie_task/movies/bloc/movie_bloc/movie_bloc.dart';
import 'package:movie_task/movies/models/movie.dart';
import 'package:movie_task/movies/view/view.dart';

class MockMovieBloc extends MockBloc<MovieEvent, MovieState>
    implements MovieBloc {}

extension on WidgetTester {
  Future<void> pumpSearchPage(MovieBloc movieBloc) => pumpWidget(
        MaterialApp(
          home: BlocProvider.value(
            value: movieBloc,
            child: const DetailsPage(),
          ),
        ),
      );
}

void main() {
  late MovieBloc movieBloc;

  setUp(() {
    movieBloc = MockMovieBloc();
  });
  testWidgets('finds details page ...', (tester) async {
    when(() => movieBloc.state).thenReturn(const MovieState());
    await tester.pumpSearchPage(movieBloc);
    expect(find.byType(DetailsPage), findsOneWidget);
  });

  testWidgets('verify Movie fetched event ...', (tester) async {
    when(() => movieBloc.state).thenReturn(const MovieState());
    await tester.pumpSearchPage(movieBloc);
    movieBloc.add(MovieFetched(const Movie(id: -1, body: '', title: '')));
    expect(find.byType(DetailsPage), findsOneWidget);
    verify(
      () =>
          movieBloc.add(MovieFetched(const Movie(id: -1, body: '', title: ''))),
    ).called(1);
  });
}
