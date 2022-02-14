import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_task/movies/bloc/movie_bloc/movie_bloc.dart';
import 'package:movie_task/movies/movies.dart';
import 'package:movie_task/nav_cubit.dart';
import 'package:optimus/optimus.dart';

class MovieListItem extends StatelessWidget {
  const MovieListItem({Key? key, required this.movie}) : super(key: key);

  final Movie movie;

  @override
  Widget build(BuildContext context) => Material(
        child: OptimusListTile(
          onTap: () {
            BlocProvider.of<NavCubit>(context).showDetails(movie);
            BlocProvider.of<MovieBloc>(context).add(MovieFetched(movie));
          },
          prefix: movie.image != null
              ? Image.network(
                  'https://image.tmdb.org/t/p/w500${movie.image}',
                  height: 100,
                  width: 100,
                )
              : null,
          title: Text(movie.title),
          subtitle: Text(movie.body),
        ),
      );
}
