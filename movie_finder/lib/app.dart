import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_finder/pages/movie_details_page.dart';
import 'package:movie_finder/pages/movie_search_page.dart';

import 'bloc/movie_details_bloc/movie_details_bloc.dart';
import 'bloc/movies_search_bloc/movies_search_bloc.dart';

class App extends StatelessWidget {
  const App({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MultiBlocProvider(
      providers: [
        BlocProvider(
          create: (context) => MoviesSearchBloc(),
        ),
        BlocProvider(
          create: (context) => MovieDetailsBloc(),
        ),
      ],
      child: MaterialApp(
        home: MovieSearchPage(),
        routes: _makeRoutes,
      ),
    );
  }

  Map<String, WidgetBuilder> get _makeRoutes => {
        MovieSearchPage.route: (_) => MovieSearchPage(),
        MovieDetailsPage.route: (_) => MovieDetailsPage(),
      };
}
