import 'package:flutter/material.dart';
import 'package:movies/config/custom_theme.dart';

class MovieCardIndex extends StatelessWidget {
  const MovieCardIndex({
    super.key,
    required this.index,
    required this.total,
  });

  final int index;
  final int total;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(4.0),
        decoration: const BoxDecoration(
          color: CustomTheme.blue,
          borderRadius: BorderRadius.only(
            topLeft: Radius.circular(8.0),
            bottomRight: Radius.circular(8.0),
          ),
        ),
        child: Text(
          '${index + 1} / $total',
          style: const TextStyle(
            fontSize: 10.0,
          ),
        ),
      );
}
