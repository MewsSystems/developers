import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:http/http.dart' as http;
import 'package:movie_task/app_navigator.dart';
import 'package:movie_task/movies/bloc/movie_bloc/movie_bloc.dart';
import 'package:movie_task/movies/bloc/search_bloc/search_bloc.dart';
import 'package:movie_task/movies/movies.dart';
import 'package:movie_task/nav_cubit.dart';

class App extends MaterialApp {
  App({Key? key})
      : super(
          key: key,
          home: RepositoryProvider(
            create: (context) => MovieRepository(httpClient: http.Client()),
            child: MultiBlocProvider(
              providers: [
                BlocProvider(
                  create: (context) => NavCubit(),
                ),
                BlocProvider(
                  create: (context) => SearchBloc(
                    movieRepository:
                        RepositoryProvider.of<MovieRepository>(context),
                  ),
                ),
                BlocProvider(
                  create: (context) => MovieBloc(
                    movieRepository:
                        RepositoryProvider.of<MovieRepository>(context),
                  ),
                ),
              ],
              child: const AppNavigator(),
            ),
          ),
        );
}
