import 'package:flutter/material.dart';
import 'package:movie_app/l10n/l10n.dart';
import 'package:movie_app/models/movie_info.dart';
import 'package:movie_app/theme/theme.dart';
import 'package:provider/provider.dart';

class MovieDetailInfo extends StatelessWidget {
  final MovieInfo movieInfo;
  const MovieDetailInfo({
    super.key,
    required this.movieInfo,
  });

  @override
  Widget build(BuildContext context) {
    final themeData = Provider.of<MovieAppThemeData>(context);
    final l10n = context.l10n;
    return SliverToBoxAdapter(
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              movieInfo.title!,
              style: themeData.movieAppTextTheme.titleLarge,
            ),
            const SizedBox(height: 8.0),
            Row(
              children: [
                Container(
                  padding: const EdgeInsets.symmetric(
                    vertical: 2.0,
                    horizontal: 8.0,
                  ),
                  decoration: BoxDecoration(
                    color: Colors.grey[300],
                    borderRadius: BorderRadius.circular(4.0),
                  ),
                  child: Text(
                    movieInfo.releaseDate!.split('-')[0],
                    style: const TextStyle(
                      fontSize: 16.0,
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                ),
                const SizedBox(width: 16.0),
                Row(
                  children: [
                    const Icon(
                      Icons.star,
                      color: Colors.amber,
                      size: 20.0,
                    ),
                    const SizedBox(width: 4.0),
                    Text(
                      (movieInfo.voteAverage! / 2).toStringAsFixed(1),
                      style: const TextStyle(
                        fontSize: 16.0,
                        fontWeight: FontWeight.w500,
                        letterSpacing: 1.2,
                      ),
                    ),
                    const SizedBox(width: 4.0),
                    Text(
                      '(${movieInfo.voteAverage})',
                      style: const TextStyle(
                        fontSize: 1.0,
                        fontWeight: FontWeight.w500,
                        letterSpacing: 1.2,
                      ),
                    ),
                  ],
                ),
                const SizedBox(width: 16.0),
                Text(
                  _showDuration(movieInfo.runtime!),
                  style: const TextStyle(
                    color: Colors.black,
                    fontSize: 16.0,
                    fontWeight: FontWeight.w500,
                    letterSpacing: 1.2,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 16.0),
            Text(
              movieInfo.overview!,
              style: themeData.movieAppTextTheme.bodyText,
            ),
            const SizedBox(height: 8.0),
            Text(
              '${l10n.genreText}: ${_showGenres(movieInfo.genres)}',
              style: themeData.movieAppTextTheme.genreText
                  .copyWith(color: Colors.black),
            ),
          ],
        ),
      ),
    );
  }
}

String _showGenres(List<Genres>? genres) {
  String result = '';
  for (var genre in genres!) {
    result += genre.name! + ', ';
  }

  if (result.isEmpty) {
    return result;
  }

  return result.substring(0, result.length - 2);
}

String _showDuration(int runtime) {
  final int hours = runtime ~/ 60;
  final int minutes = runtime % 60;

  if (hours > 0) {
    return '${hours}h ${minutes}m';
  } else {
    return '${minutes}m';
  }
}
