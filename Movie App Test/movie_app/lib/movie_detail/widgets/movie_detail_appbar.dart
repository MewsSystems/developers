import 'package:flutter/material.dart';
import 'package:movie_app/data/api_services.dart';
import 'package:movie_app/models/movie_info.dart';
import 'package:transparent_image/transparent_image.dart';

class MovieDetailAppBar extends StatelessWidget {
  final MovieInfo movieInfo;
  const MovieDetailAppBar({
    super.key,
    required this.movieInfo,
  });

  @override
  Widget build(BuildContext context) {
    return SliverAppBar(
      pinned: true,
      expandedHeight: 250.0,
      flexibleSpace: FlexibleSpaceBar(
        background: Hero(
          tag: "${movieInfo.id}",
          child: movieInfo.posterPath != null
              ? FadeInImage.memoryNetwork(
                  image: TheMovieDbService.imageUrl(
                      movieInfo.posterPath!, PosterSize.w780),
                  placeholder: kTransparentImage,
                  fit: BoxFit.cover,
                )
              : const SizedBox.shrink(),
        ),
      ),
      leading: IconButton(
        key: const Key('backButton'),
        icon: const Icon(Icons.keyboard_arrow_left_rounded),
        iconSize: 30,
        onPressed: () {
          Navigator.pop(context);
        },
      ),
    );
  }
}
