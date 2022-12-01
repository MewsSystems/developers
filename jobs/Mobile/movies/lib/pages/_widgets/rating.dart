import 'package:flutter/material.dart';
import 'package:flutter_rating_bar/flutter_rating_bar.dart';

class Rating extends StatelessWidget {
  const Rating({super.key, required this.rating});

  final double rating;

  @override
  Widget build(BuildContext context) => RatingBarIndicator(
        rating: rating / 2,
        itemBuilder: (context, index) => const Icon(
          Icons.star,
          color: Colors.amber,
        ),
        itemCount: 5,
        itemSize: 20.0,
        direction: Axis.horizontal,
      );
}
