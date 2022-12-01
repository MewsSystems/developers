import 'package:flutter/material.dart';
import 'package:movies/config/consts.dart';
import 'package:movies/config/custom_theme.dart';
import 'package:movies/models/movie_model.dart';
import 'package:movies/pages/_widgets/poster.dart';
import 'package:movies/pages/_widgets/rating.dart';
import 'package:movies/pages/_widgets/votes.dart';
import 'package:movies/pages/movie_page.dart/movie_page.dart';
import 'package:movies/pages/search_page/_widgets/movie_card_index.dart';
import 'package:movies/pages/search_page/_widgets/no_image.dart';

class MovieCard extends StatelessWidget {
  const MovieCard({
    super.key,
    required this.movie,
    required this.index,
    required this.totalPages,
  });

  final Movie movie;
  final int index;
  final int totalPages;

  @override
  Widget build(BuildContext context) => Material(
        color: CustomTheme.black,
        child: InkWell(
          splashColor: CustomTheme.blue.withOpacity(.5),
          highlightColor: CustomTheme.blue.withOpacity(.5),
          child: Padding(
            padding:
                const EdgeInsets.symmetric(horizontal: 16.0, vertical: 8.0),
            child: Row(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Stack(
                  children: [
                    if (movie.posterPath != null)
                      Poster(
                        width: 100.0,
                        height: 150.0,
                        path: '${Images.w300}${movie.posterPath}',
                      )
                    else
                      const NoImage(height: 150.0, width: 100.0),
                    MovieCardIndex(index: index, total: totalPages),
                  ],
                ),
                const SizedBox(width: 16.0),
                Expanded(
                  child: Container(
                    padding: const EdgeInsets.symmetric(vertical: 8.0),
                    height: 150.0,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          movie.originalTitle,
                          maxLines: 3,
                          style: const TextStyle(
                            fontSize: 18.0,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                        const SizedBox(height: 4.0),
                        Text(
                          movie.releaseDate != null
                              ? 'Relese: ${movie.releaseDate}'
                              : 'No release date',
                          style: const TextStyle(fontSize: 12.0),
                        ),
                        const Spacer(),
                        Rating(rating: movie.voteAverage),
                        const SizedBox(height: 4.0),
                        Votes(count: movie.voteCount),
                      ],
                    ),
                  ),
                ),
              ],
            ),
          ),
          onTap: () => Navigator.of(context).push(
            MaterialPageRoute<dynamic>(
              builder: (BuildContext context) => MoviePage(movieId: movie.id),
            ),
          ),
        ),
      );
}
