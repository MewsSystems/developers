import 'package:flutter/material.dart';

import '../../../common/widgets/smart_image.dart';
import '../models/movie.dart';

class MovieListItem extends StatelessWidget {
  final Movie movie;

  const MovieListItem({required this.movie, Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      clipBehavior: Clip.antiAlias,
      margin: const EdgeInsets.symmetric(horizontal: 20, vertical: 8),
      padding: const EdgeInsets.fromLTRB(2, 2, 2, 8),
      decoration: const BoxDecoration(
        color: Colors.grey,
        borderRadius: BorderRadius.only(
          bottomLeft: Radius.circular(12),
          bottomRight: Radius.circular(12),
        ),
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
