import 'package:flutter/material.dart';
import 'package:movie_finder/components/movie_details_mixin.dart';
import 'package:movie_finder/data/model/movies/movie.dart';
import 'package:movie_finder/utils/constants.dart';

class MovieTile extends StatelessWidget with MovieDetailsMixin {
  const MovieTile({
    Key? key,
    required this.movie,
    required this.onPressed,
  }) : super(key: key);

  final Movie movie;
  final VoidCallback onPressed;

  @override
  Widget build(BuildContext context) {
    return _buildBase(context);
  }

  Widget _buildBase(BuildContext context) {
    return InkWell(
      onTap: onPressed,
      child: Container(
        padding: const EdgeInsets.all(16),
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(7),
          color: Colors.white,
          boxShadow: kShadow,
        ),
        constraints: const BoxConstraints(
          minHeight: 64,
          maxHeight: 256,
        ),
        child: _buildContent(context),
      ),
    );
  }

  Widget _buildContent(BuildContext context) {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        if (movie.posterPath != null) ...[
          buildImage(posterPath: movie.posterPath!, height: 120, boxFit: BoxFit.fitHeight),
          kHorizontal16
        ],
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            mainAxisSize: MainAxisSize.min,
            children: [
              buildTitle(
                context,
                title: movie.title ?? 'Dummy Title',
              ),
              if (movie.releaseDate != null && movie.releaseDate!.isNotEmpty) ...[
                kVertical8,
                buildReleaseDate(context, releaseDate: movie.releaseDate!),
              ],
              if (movie.overview != null && movie.overview!.isNotEmpty) ...[
                kVertical16,
                Flexible(
                    child: buildOverview(
                  context,
                  overview: movie.overview!,
                  maxLines: 3,
                )),
              ]
            ],
          ),
        ),
      ],
    );
  }
}
