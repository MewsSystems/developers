import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_task/movies/bloc/movie_bloc/movie_bloc.dart';
import 'package:movie_task/movies/movies.dart';
import 'package:movie_task/nav_cubit.dart';

class AppNavigator extends StatelessWidget {
  const AppNavigator({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) => BlocBuilder<NavCubit, Movie>(
        builder: (context, movie) => Navigator(
          pages: [
            const MaterialPage<dynamic>(
              child: SearchPage(),
            ),
            if (movie.id != -1)
              const MaterialPage<dynamic>(
                child: DetailsPage(),
              ),
          ],
          onPopPage: (route, dynamic result) {
            BlocProvider.of<NavCubit>(context).popToSearch();
            BlocProvider.of<MovieBloc>(context).add(MovieDetailsPopped());

            return route.didPop(result);
          },
        ),
      );
}
