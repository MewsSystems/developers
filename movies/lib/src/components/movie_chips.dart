import 'package:flutter/material.dart';
import 'package:movies/constants.dart';
import 'package:movies/src/components/movie_chip.dart';
import 'package:movies/src/model/movie.dart';

class MovieChips extends StatelessWidget {
  const MovieChips({Key? key, required this.movie}) : super(key: key);

  final Movie movie;

  @override
  Widget build(BuildContext context) => Wrap(
        spacing: 8,
        children: [
          MovieChip(label: movie.releaseDate.year.toString()),
          MovieChip(
            label: movie.voteAverage.toString(),
            icon: Icons.star,
          ),
          MovieChip(
            label: '${movie.popularity.toStringAsFixed(1)} %',
            icon: Icons.local_fire_department,
          ),
        ],
      );
}
