import 'package:flutter/material.dart';
import 'package:flutter_rating_bar/flutter_rating_bar.dart';
import 'package:movies/config/custom_theme.dart';
import 'package:movies/models/movie_model.dart';
import 'package:movies/pages/movie_page.dart/movie_page.dart';
import 'package:movies/pages/search_page/_widgets/movie_card_index.dart';
import 'package:movies/pages/search_page/_widgets/network_image_loader.dart';
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
                      NetworkImageLoader(
                        width: 100.0,
                        height: 150.0,
                        path:
                            'https://image.tmdb.org/t/p/w300/${movie.posterPath}',
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
                          movie.releaseDate ?? 'no release date',
                          style: const TextStyle(fontSize: 12.0),
                        ),
                        const Spacer(),
                        RatingBarIndicator(
                          rating: movie.voteAverage / 2,
                          itemBuilder: (context, index) => const Icon(
                            Icons.star,
                            color: Colors.amber,
                          ),
                          itemCount: 5,
                          itemSize: 20.0,
                          direction: Axis.horizontal,
                        ),
                        const SizedBox(height: 4.0),
                        Text(
                          'Votes: ${movie.voteCount}',
                          style: const TextStyle(fontSize: 12.0),
                        ),
                      ],
                    ),
                  ),
                ),
              ],
            ),
          ),
          onTap: () => Navigator.of(context).push(
            MaterialPageRoute<dynamic>(
              builder: (BuildContext context) => const MoviePage(),
            ),
          ),
        ),
      );
}
