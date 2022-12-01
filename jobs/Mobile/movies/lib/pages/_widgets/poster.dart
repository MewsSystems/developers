import 'package:flutter/material.dart';
import 'package:movies/pages/_widgets/network_image_loader.dart';

class Poster extends StatelessWidget {
  const Poster({
    super.key,
    required this.height,
    required this.width,
    required this.path,
  });

  final double height;
  final double width;
  final String path;

  @override
  Widget build(BuildContext context) => SizedBox(
        height: height,
        width: width,
        child: ClipRRect(
          borderRadius: const BorderRadius.all(Radius.circular(8.0)),
          child: NetworkImageLoader(path: path),
        ),
      );
}
