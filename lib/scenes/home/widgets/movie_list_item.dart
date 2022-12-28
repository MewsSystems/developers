import 'package:flutter/material.dart';

import '../../../common/widgets/smart_image.dart';
import '../models/movie.dart';
import 'vote_indicator.dart';

class MovieListItem extends StatelessWidget {
  final Movie movie;

  const MovieListItem({required this.movie, Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      clipBehavior: Clip.antiAlias,
      margin: const EdgeInsets.symmetric(horizontal: 20, vertical: 8),
      padding: const EdgeInsets.fromLTRB(2, 2, 2, 0),
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
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: Row(
              children: [
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text(
                        movie.title,
                        textAlign: TextAlign.center,
                        style: Theme.of(context).textTheme.headline6,
                      ),
                      if (movie.releaseDate != null) Text(movie.releaseDate!)
                    ],
                  ),
                ),
                VoteIndicator(
                    progress: movie.voteAverage / 10,
                    voteCount: movie.voteCount),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
