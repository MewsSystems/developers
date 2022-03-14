import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mews_imdb/search/views/search_page.dart';
import 'package:movie_repository/movie_repository.dart';

class IMDbApp extends StatelessWidget {
  const IMDbApp({Key? key, required MovieRepository movieRepository})
      : _movieRepository = movieRepository,
        super(key: key);

  final MovieRepository _movieRepository;

  @override
  Widget build(BuildContext context) => MaterialApp(
        title: 'IMDb Search',
        theme: ThemeData(
          primaryColor: const Color.fromRGBO(51, 60, 78, 1),
          appBarTheme: const AppBarTheme(
            backgroundColor: Color.fromRGBO(51, 60, 78, 1),
          ),
        ),
        home: RepositoryProvider(
          create: (context) => _movieRepository,
          child: const SearchPage(),
        ),
      );
}
