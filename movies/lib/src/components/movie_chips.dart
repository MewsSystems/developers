import 'package:flutter/material.dart';
import 'package:movies/src/components/movie_chip.dart';
import 'package:movies/src/model/movie/movie.dart';

/// Wrap that generates [MovieChip]s according to the data of a [movie]
class MovieChips extends StatelessWidget {
  const MovieChips({Key? key, required this.movie}) : super(key: key);

  final Movie movie;

  @override
  Widget build(BuildContext context) => Wrap(
        spacing: 8,
        children: [
          if (movie.releaseDate != null)
            MovieChip(label: movie.releaseDate!.year.toString()),
          MovieChip(
            label: movie.voteAverage.toString(),
            icon: Icons.star,
            iconColor: Theme.of(context).colorScheme.primary,
          ),
          MovieChip(
            label: '${movie.popularity.toStringAsFixed(1)} %',
            icon: Icons.local_fire_department,
            iconColor: const Color(0xFFEC7505),
          ),
        ],
      );
}
