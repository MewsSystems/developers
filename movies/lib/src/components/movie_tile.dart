import 'package:flutter/material.dart';
import 'package:movies/src/components/genre_chips.dart';
import 'package:movies/src/components/movie_chips.dart';
import 'package:movies/src/components/poster.dart';
import 'package:movies/src/model/movie/movie.dart';

/// Used to show data about a [Movie] in a rectangular shape that fills the parent width
class MovieTile extends StatelessWidget {
  const MovieTile({
    Key? key,
    required this.movie,
    required this.titleSize,
    this.posterHeight,
    this.showOriginalTitle = false,
  }) : super(key: key);

  final Movie movie;
  final double titleSize;
  final double? posterHeight;
  final bool showOriginalTitle;

  @override
  Widget build(BuildContext context) => Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Poster(
            url: movie.smallPoster,
            heroTag: movie.id,
            height: posterHeight,
          ),
          const SizedBox(width: 16),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  movie.title,
                  style: TextStyle(
                    fontSize: titleSize,
                  ),
                ),
                if (showOriginalTitle && movie.title != movie.originalTitle)
                  Text('(${movie.originalTitle})'),
                const SizedBox(height: 4),
                GenreChips(genreIds: movie.genreIds),
                MovieChips(movie: movie),
              ],
            ),
          )
        ],
      );
}
