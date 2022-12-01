import 'package:flutter/material.dart';
import 'package:movies/config/custom_theme.dart';

class NoImage extends StatelessWidget {
  const NoImage({
    super.key,
    required this.height,
    required this.width,
  });

  final double height;
  final double width;

  @override
  Widget build(BuildContext context) => Container(
        height: height,
        width: width,
        decoration: const BoxDecoration(
          color: CustomTheme.blue,
          borderRadius: BorderRadius.all(
            Radius.circular(8.0),
          ),
        ),
        child: const Center(
          child: Text('no image'),
        ),
      );
}
