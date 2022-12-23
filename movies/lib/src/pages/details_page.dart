import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:movies/constants.dart';

/// Displays detailed information about a movie
class DetailsPage extends StatelessWidget {
  DetailsPage({
    super.key,
  });

  static const routeName = '/sample_item';

  final movie = items.first;

  @override
  Widget build(BuildContext context) => Scaffold(
        body: Stack(
          children: [
            Column(
              children: [
                Hero(
                  tag: movie.id,
                  child: CachedNetworkImage(imageUrl: movie.posterUrl),
                ),
                Hero(
                  tag: movie.id + movie.title,
                  child: Text(movie.title, style: const TextStyle(fontSize: 32),),
                ),
              ],
            ),
            Positioned(
              top: 8,
              left: 8,
              child: SafeArea(
                child: FloatingActionButton.small(
                  child: const Icon(Icons.arrow_back),
                  onPressed: () => Navigator.pop(context),
                ),
              ),
            )
          ],
        ),
      );
}
