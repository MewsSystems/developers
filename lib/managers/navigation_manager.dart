import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../scenes/home/bloc/home_bloc.dart';
import '../scenes/home/models/movie.dart';
import '../scenes/home/pages/home_page.dart';
import '../scenes/home/repository/home_repository.dart';
import '../scenes/movie_detail/bloc/movie_detail_bloc.dart';
import '../scenes/movie_detail/pages/movie_detail_page.dart';
import 'get_it.dart';

class NavigationManager {}

extension NavigationManagerPages on NavigationManager {
  Widget get homePage => BlocProvider(
        create: (context) => HomeBloc(repository: getIt.get<HomeRepository>()),
        child: const HomePage(),
      );
}

extension NavigationManagerNav on NavigationManager {
  Future<dynamic> navigateToMovieDetail(
      {required BuildContext context, required Movie movie}) async {
    final destination = BlocProvider(
      create: (context) => MovieDetailBloc(movie: movie),
      child: const MovieDetailPage(),
    );

    return Navigator.of(context)
        .push(MaterialPageRoute(builder: (context) => destination));
  }
}
