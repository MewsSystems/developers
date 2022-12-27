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
        children: [
          SmartImage(
            imageUrl: movie.imageUrl,
            height: 150,
          )
        ],
      ),
    );
  }
}
