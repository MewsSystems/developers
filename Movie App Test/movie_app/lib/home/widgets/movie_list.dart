import 'package:flutter/material.dart';
import 'package:movie_app/models/movie.dart';

class MovieList extends StatelessWidget {
  final List<Movie> moviesList;
  const MovieList({
    required this.moviesList,
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      shrinkWrap: true,
      itemCount: moviesList.length,
      itemBuilder: ((context, index) {
        return Card(
          child: ListTile(
            title: Text(moviesList[index].title!),
          ),
        );
      }),
    );
  }
}
