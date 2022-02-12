import 'package:flutter/material.dart';
import 'package:movie_task/movies/movies.dart';
import 'package:optimus/optimus.dart';

class MovieListItem extends StatelessWidget {
  const MovieListItem({Key? key, required this.movie}) : super(key: key);

  final Movie movie;

  @override
  Widget build(BuildContext context) {
    final textTheme = Theme.of(context).textTheme;

    return Material(
      child: OptimusListTile(
        onTap: () {},
        prefix: Text('${movie.id}', style: textTheme.caption),
        title: Text(movie.title),
        subtitle: Text(movie.body),
      ),
    );
  }
}
