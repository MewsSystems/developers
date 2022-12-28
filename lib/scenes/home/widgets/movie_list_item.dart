import 'package:flutter/material.dart';

import '../../../common/widgets/smart_image.dart';
import '../models/movie.dart';

class MovieListItem extends StatelessWidget {
  final Movie movie;

  const MovieListItem({required this.movie, Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: const EdgeInsets.symmetric(horizontal: 20, vertical: 8),
      padding: const EdgeInsets.all(8),
      decoration: BoxDecoration(
        color: Colors.grey,
        borderRadius: BorderRadius.circular(12),
      ),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          SmartImage(
            imageUrl: movie.imageUrl,
            height: MediaQuery.of(context).size.height / 1.5,
          ),
          Text(movie.title, style: Theme.of(context).textTheme.headline6),
          Text(movie.popularity.toString()),
          Text(movie.voteAverage.toString()),
          Text(movie.voteCount.toString()),
        ],
      ),
    );
  }
}
